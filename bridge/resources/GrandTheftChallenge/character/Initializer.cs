using GrandTheftChallenge.Data;
using GrandTheftChallenge.Data.Model;
using GrandTheftChallenge.games.Sumo;
using GrandTheftChallenge.Games;
using GrandTheftChallenge.Utility;
using GTANetworkAPI;
using System.Collections.Generic;

namespace GrandTheftChallenge.Character
{
    public class Initializer : Script
    {
        [ServerEvent(Event.PlayerConnected)]
        public async void PlayerConnectedAsyncEvent(Client player)
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
                // Check if the player can log into the game
                CheckAccountState(player, account);
            }
        }

        [RemoteEvent("LoginAccount")]
        public async void LoginAccountEvent(Client player, string username, string password)
        {
            // Check if the account exists
            bool accountCorrect = await DatabaseHandler.LoginAccount(username, password);

            if(!accountCorrect)
            {
                // The creadentials don't match
                player.TriggerEvent("WarnLoginFailed");
                return;
            }

            // Show the main menu to the player
            player.TriggerEvent("ShowSkinSelector");
        }

        [RemoteEvent("RegisterAccount")]
        public async void RegisterAccountEvent(Client player, string username, string email, string password)
        {
            // Try to register the account
            int registeredAccount = await DatabaseHandler.RegisterAccount(username, email, password, player.SocialClubName).ConfigureAwait(false);

            if(registeredAccount == 0)
            {
                // The account is already registered
                player.TriggerEvent("WarnRegisteredAccount");
                return;
            }

            // Log the player into the server
            player.TriggerEvent("DestroyConnectionBrowser");
        }

        [RemoteEvent("SkinSelection")]
        public void SkinSelection(Client player, int skin)
        {
            // Get the hash of the skin and set to the player
            player.SetSkin((PedHash)skin);

            // Send to the game selection
            player.TriggerEvent("ShowMenuWindow");
        }

        [RemoteEvent("GameSelection")]
        public void GameSelection(Client player, int game)
        {
            //Destroy current CEF Browser, camera and return radar
            player.TriggerEvent("DestroyConnectionBrowser");
            player.TriggerEvent("DestroyCam");

            // Create the lobby
            LobbyModel lobby = new LobbyModel()
            {
                Id = 0,
                Countdown = 0,
                Players = new List<Client>(),
                Track = 1
            };

            // Add the lobby to the list
            GamesHandler.lobbyList.Add(lobby);

            // Switch to the game chosen
            // Derby = 1
            switch (game)
            {
                case 1:
                    // Iniciate Derby
                    Games.Derby.Initializer.StartDerby(player);
                    GamesHandler.lobbyList[0].Players.Add(player);
                    GamesHandler.StartCountdown(GamesHandler.lobbyList[0].Id);
                    break;

                case 2:
                    Games.CVH.Initializer.StartCVH(player);
                    break;

                case 3:
                    SumoStartupHandler.Initialize(lobby);
                    break;
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
                    // Show the login screen
                    player.TriggerEvent("ShowLoginWindow");
                    break;
            }
        }
    }
}
