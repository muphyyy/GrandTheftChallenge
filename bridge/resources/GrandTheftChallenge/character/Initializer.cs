using GrandTheftChallenge.Data;
using GrandTheftChallenge.Data.Model;
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
                CheckAccountState(account);
            }
        }
        
        private void InitializeCharacterData() { }

        private void CheckAccountState(AccountModel account)
        {

        }
    }
}
