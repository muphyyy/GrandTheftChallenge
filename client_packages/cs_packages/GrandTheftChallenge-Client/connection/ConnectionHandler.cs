using RAGE;
using RAGE.Elements;

namespace GrandTheftChallenge_Client.Connection
{
    public class ConnectionHandler : Events.Script
    {
        public ConnectionHandler()
        {
            // Register custom events

            // Register RAGE's events
            Events.OnGuiReady += OnGuiReadyEvent;
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
