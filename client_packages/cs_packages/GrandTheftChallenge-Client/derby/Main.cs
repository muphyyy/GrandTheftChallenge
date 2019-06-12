using System;
using System.Collections.Generic;
using System.Text;
using RAGE;

namespace GrandTheftChallenge_Client.derby
{
    public class Main : Events.Script
    {
        public Main()
        {
            Events.Add("DerbyFreezePlayer", DerbyFreezePlayerEvent);
        }

        private static bool freeze;
        private void DerbyFreezePlayerEvent(object[] args)
        {
            // Get the status of the freeze (0 = false, 1 = true)
            int status = (int)args[0];
            if (status == 0) freeze = false;
            else freeze = true;

            RAGE.Elements.Player.LocalPlayer.FreezePosition(freeze);
        }
    }
}
