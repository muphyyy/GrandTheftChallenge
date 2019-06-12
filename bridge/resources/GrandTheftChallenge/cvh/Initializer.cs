using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace GrandTheftChallenge.cvh
{
    public class Initializer : Script
    {
        public static void StartCVH(Client player)
        {
            // Set the dimension of the game
            player.Dimension = 2;
        }
    }
}
