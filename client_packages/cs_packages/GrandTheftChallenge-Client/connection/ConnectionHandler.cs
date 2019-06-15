using RAGE;
using RAGE.Ui;
using RAGE.Elements;
using GrandTheftChallenge_Client.Browser;
using System;

namespace GrandTheftChallenge_Client.Connection
{
    public class ConnectionHandler : Events.Script
    {
        private static HtmlWindow browser = null;
        private static int camera;

        public ConnectionHandler()
        {
            // Server to client events
            Events.Add("ShowRegisterWindow", ShowRegisterWindowEvent);
            Events.Add("ShowLoginWindow", ShowLoginWindowEvent);
            Events.Add("ShowPlayerBan", ShowPlayerBanEvent);
            Events.Add("ShowSkinSelector", ShowSkinSelectorEvent);
            Events.Add("ShowMenuWindow", ShowMenuWindowEvent);
            Events.Add("ShowCVHWindow", ShowCVHWindowEvent);
            Events.Add("ShowLoadingWindow", ShowLoadingWindowEvent);
            Events.Add("DestroyConnectionBrowser", DestroyConnectionBrowserEvent);
            Events.Add("DestroyCam", DestroyCamEvent);

            // Client to server events
            Events.Add("RegisterServer", RegisterServerEvent);
            Events.Add("LoginServer", LoginServerEvent);
            Events.Add("SkinSelectionUser", SkinSelectionUserEvent);
            Events.Add("GameSelectionUser", GameSelectionUserEvent);
            Events.Add("CVHSelectionTeam", CVHSelectionTeamEvent);

            // Register RAGE's events
            Events.OnGuiReady += OnGuiReadyEvent;
        }

        private void ShowLoadingWindowEvent(object[] args)
        {
            // Create the browser
            browser = BrowserHandler.CreateBrowser("package://statics/loading.html", null);
        }

        private void ShowCVHWindowEvent(object[] args)
        {
            // Create the camera
            camera = RAGE.Game.Cam.CreateCameraWithParams(RAGE.Game.Misc.GetHashKey("DEFAULT_SCRIPTED_CAMERA"), 3400.0f, 5075.0f, 20.0f, 0.0f, 0.0f, 8.0f, 75.0f, true, 2);
            RAGE.Game.Cam.SetCamActive(camera, true);
            RAGE.Game.Cam.RenderScriptCams(true, false, 0, false, false, 0);

            // Disable chat
            Chat.Show(false);

            //Disable the radar
            RAGE.Game.Ui.DisplayRadar(false);

            // Create the menu browser
            browser = BrowserHandler.CreateBrowser("package://statics/cvh.html", null);
        }

        private void CVHSelectionTeamEvent(object[] args)
        {
            // Define the param from the CEF
            int team = Convert.ToInt32(args[0]);

            // Call the server to choose a team
            Events.CallRemote("CHVSelection", team);

            // Enable chat
            Chat.Show(true);
        }

        private void GameSelectionUserEvent(object[] args)
        {
            // Define the param from the CEF
            int game = Convert.ToInt32(args[0]);

            // Call the server to start a game
            Events.CallRemote("GameSelection", game);
        }

        private void ShowMenuWindowEvent(object[] args)
        {
            // Destroy the previous window
            BrowserHandler.DestroyBrowser(browser);

            // Create the menu browser
            browser = BrowserHandler.CreateBrowser("package://statics/gameselector.html", null);
        }

        private void SkinSelectionUserEvent(object[] args)
        {
            // Define the param from the CEF
            int skin = Convert.ToInt32(args[0]);

            // Call the server to set the skin
            Events.CallRemote("SkinSelection", skin);
        }

        private void DestroyCamEvent(object[] args)
        {
            // Destroy cam and radar
            RAGE.Game.Cam.RenderScriptCams(false, false, 0, true, false, 0);
            RAGE.Game.Ui.DisplayRadar(true);

            // Active chat
            Chat.Show(true);
        }

        private void ShowLoginWindowEvent(object[] args)
        {
            // Create the login browser
            browser = BrowserHandler.CreateBrowser("package://statics/login.html", null);
        }

        private void ShowRegisterWindowEvent(object[] args)
        {
            // Create the register browser
            browser = BrowserHandler.CreateBrowser("package://statics/register.html", null);
        }

        private void ShowPlayerBanEvent(object[] args)
        {
            // Create the player ban browser
            browser = BrowserHandler.CreateBrowser("package://statics/ban.html", new object[] { "sendBanReason", args[0].ToString() });
        }

        private void ShowSkinSelectorEvent(object[] args)
        {
            // Destroy the previous broswer
            DestroyConnectionBrowserEvent(null);
        
            // Create the menu browser
            browser = BrowserHandler.CreateBrowser("package://statics/skinselector.html", null);
        }

        private void DestroyConnectionBrowserEvent(object[] args)
        {
            if (browser == null) return;

            // Destroy the active CEF window
            BrowserHandler.DestroyBrowser(browser);
            browser = null;

            // Unfreeze the player
            Player.LocalPlayer.FreezePosition(false);
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

            // Create the camera
            var cam = RAGE.Game.Cam.CreateCameraWithParams(RAGE.Game.Misc.GetHashKey("DEFAULT_SCRIPTED_CAMERA"), 3400.0f, 5075.0f, 20.0f, 0.0f, 0.0f, 8.0f, 75.0f, true, 2);
            RAGE.Game.Cam.SetCamActive(cam, true);
            RAGE.Game.Cam.RenderScriptCams(true, false, 0, false, false, 0);

            // Hide the chat, HUD and radar
            Chat.Show(false);
            Chat.Activate(false);
            RAGE.Game.Ui.DisplayHud(false);
            RAGE.Game.Ui.DisplayRadar(false);
        }
    }
}
