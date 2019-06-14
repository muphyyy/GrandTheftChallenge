using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace GrandTheftChallenge.Games.CVH
{
    public class Initializer : Script
    {
        private static int cars = 0;
        private static int humans = 0;
        public static void StartCVH(Client player)
        {
            // Start the CVH choose team browser
            player.TriggerEvent("ShowCVHWindow");

            // Set the dimension of the minigame
            player.Dimension = 2;
        }

        [RemoteEvent("CHVSelection")]
        public void RE_CHVSelection(Client player, int team)
        {
            // Destroy camera, broswer and radar
            player.TriggerEvent("DestroyConnectionBrowser");
            player.TriggerEvent("DestroyCam");
        }

    }
}
