using GrandTheftChallenge.Data;
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
                // Show the login screen
                player.TriggerEvent("ShowLoginWindow");
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
            // Load the account from the player's Social Name
            AccountModel account = await DatabaseHandler.GetPlayerAccount(player.SocialClubName).ConfigureAwait(false);

            // Check the status of the account
            CheckAccountState(player, account);
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
        public void SkinSelection(Client player, string skin)
        {
            // Get the hash of the skin and set to the player
            uint hash = NAPI.Util.GetHashKey(skin);
            player.SetSkin(hash);

            //Destroy current CEF Browser, camera and return radar
            player.TriggerEvent("DestroyConnectionBrowser");
            player.TriggerEvent("DestroyCam");


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
                    //Destroy current CEF Browser
                    player.TriggerEvent("DestroyConnectionBrowser");

                    // Show the main menu to the player
                    player.TriggerEvent("ShowSkinSelector");
                    break;
            }
        }
    }
}
