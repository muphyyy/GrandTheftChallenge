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

            // Set the position where player starts
            player.Position = new Vector3(1660.336, 234.5094, 408.6269);

            // Freeze player
            player.TriggerEvent("DerbyFreezePlayer", 1);

            // Delay 10 seconds (just to make sure that the map loads correctly)
            await Task.Delay(10000);
            player.SendChatMessage("ya");

            // Unfreeze player
            player.TriggerEvent("DerbyFreezePlayer", 0);
        }
    }
}
