using RAGE;
using RAGE.Ui;
using RAGE.Elements;
using GrandTheftChallenge_Client.Browser;

namespace GrandTheftChallenge_Client.Connection
{
    public class ConnectionHandler : Events.Script
    {
        private static HtmlWindow browser = null;

        public ConnectionHandler()
        {
            // Server to client events
            Events.Add("ShowRegisterWindow", ShowRegisterWindowEvent);
            Events.Add("ShowPlayerBan", ShowPlayerBanEvent);
            Events.Add("ShowMainMenu", ShowMainMenuEvent);
            Events.Add("DestroyConnectionBrowser", DestroyConnectionBrowserEvent);

            // Client to server events
            Events.Add("RegisterServer", RegisterServerEvent);
            Events.Add("LoginServer", LoginServerEvent);

            // Register RAGE's events
            Events.OnGuiReady += OnGuiReadyEvent;
        }

        private void ShowRegisterWindowEvent(object[] args)
        {
            // Create the register browser
            browser = BrowserHandler.CreateBrowser("package://statics/assets/register.html", null);
        }

        private void ShowPlayerBanEvent(object[] args)
        {
            // Create the player ban browser
            browser = BrowserHandler.CreateBrowser("package://statics/assets/ban.html", null);
        }

        private void ShowMainMenuEvent(object[] args)
        {
            // Create the menu browser
            browser = BrowserHandler.CreateBrowser("package://statics/assets/menu.html", null);
        }

        private void DestroyConnectionBrowserEvent(object[] args)
        {
            if (browser == null) return;

            // Destroy the active CEF window
            BrowserHandler.DestroyBrowser(browser);
            browser = null;
        }

        private void RegisterServerEvent(object[] args)
        {
            // Get the parameters from the web browser
            string username = args[0].ToString();
            string email = args[1].ToString();
            string password = args[2].ToString();

            // Call the server to register the new player
            Events.CallRemote("RegisterAccount", username, email, password);
        }

        private void LoginServerEvent(object[] args)
        {
            // Get the parameters from the web browser
            string username = args[0].ToString();
            string password = args[1].ToString();

            // Call the server to login the player
            Events.CallRemote("LoginAccount", username, password);
        }

        private void OnGuiReadyEvent()
        {
            // Initialize the player's position
            Player.LocalPlayer.FreezePosition(true);

            // Hide the chat and the HUD
            Chat.Show(false);
            Chat.Activate(false);
            RAGE.Game.Ui.DisplayHud(false);
        }
    }
}
