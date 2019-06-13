using RAGE;

namespace GrandTheftChallenge_Client.Derby
{
    public class Main : Events.Script
    {
        private static bool freeze;

        public Main()
        {
            Events.Add("DerbyFreezePlayer", DerbyFreezePlayerEvent);
        }

        private void DerbyFreezePlayerEvent(object[] args)
        {
            // Get the status of the freeze (0 = false, 1 = true)
            int status = (int)args[0];
            freeze = status != 0;

            RAGE.Elements.Player.LocalPlayer.FreezePosition(freeze);
        }
    }
}
