﻿using GrandTheftChallenge.Data;
using GrandTheftChallenge.Data.Model;
using GrandTheftChallenge.Utility;
using GTANetworkAPI;

namespace GrandTheftChallenge.Character
{
    public class Initializer : Script
    {
        [ServerEvent(Event.PlayerConnected)]
        private async void PlayerConnectedAsyncEvent(Client player)
        {
            // Initialize the character data
            InitializeCharacterData();

            // Load the account from the player's Social Name
            AccountModel account = await DatabaseHandler.GetPlayerAccount(player.SocialClubName).ConfigureAwait(false);

            if(account == null)
            {
                // Show the register screen
                player.TriggerEvent("ShowRegisterWindow");
            }
            else
            {
                // Check the account
                CheckAccountState(player, account);
            }
        }
        
        private void InitializeCharacterData() { }

        private async void CheckAccountState(Client player, AccountModel account)
        {
            switch(account.State)
            {
                case Constants.ACCOUNT_STATE_BANNED:
                    // Get the account's ban reason
                    string reason = await DatabaseHandler.GetAccountBanReason(account.Id);

                    // The account has been banned, we show the message to the player
                    player.TriggerEvent("ShowPlayerBan", reason);
                    break;

                case Constants.ACCOUNT_STATE_PLAYABLE:
                    // Show the main menu to the player
                    player.TriggerEvent("ShowMainMenu");
                    break;
            }
        }
    }
}
