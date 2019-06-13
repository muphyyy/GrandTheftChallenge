using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using System.Threading.Tasks;

namespace GrandTheftChallenge.derby
{
    public class Initializer : Script
    {
        public static async void StartDerby(Client player)
        {
            // Set the dimension of the game
            player.Dimension = 1;

            // Show loading screen
            player.TriggerEvent("ShowLoadingWindow");

            // Set the position where player starts
            Vector3 position = new Vector3(1660.336, 234.5094, 408.6269);
            player.Position = position.Subtract(new Vector3(0, 0, -0.5));

            // Freeze player
            player.TriggerEvent("DerbyFreezePlayer", 1);

            // Delay 6 seconds (just to make sure that the map loads correctly)
            await Task.Delay(6000);

            // Destroy the loading browser
            player.TriggerEvent("DestroyConnectionBrowser");

            // Unfreeze player
            player.TriggerEvent("DerbyFreezePlayer", 0);
        }
    }
}
