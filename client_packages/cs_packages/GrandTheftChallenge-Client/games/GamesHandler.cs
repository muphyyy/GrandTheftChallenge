using System;
using System.Collections.Generic;
using RAGE;
using RAGE.Elements;

namespace GrandTheftChallenge_Client.Games
{
    public class GamesHandler : Events.Script
    {
        private int countdownSeconds = 0;
        private int countdownScaleform = -1;

        public GamesHandler()
        {
            Events.Add("ShowCountdown", ShowCountdownEvent);

            // Add default RAGE's events
            Events.Tick += TickEvent;

            // Get the scaleform for the countdown
            countdownScaleform = RAGE.Game.Graphics.RequestScaleformMovie("countdown");
        }

        private void ShowCountdownEvent(object[] args)
        {
            // Get the time to show and check if it's the first tick
            countdownSeconds = Convert.ToInt32(args[0]);
            bool justJoined = Convert.ToBoolean(args[1]);

            if(justJoined)
            {
                // Toggle startup freeze
                ToggleStartupFreeze(true);
            }

            if (countdownSeconds > 0)
            {
                // Set the countdown time
                RAGE.Game.Graphics.PushScaleformMovieFunction(countdownScaleform, "FADE_MP");
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterString(countdownSeconds.ToString());
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(255);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(255);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(255);
                RAGE.Game.Graphics.PopScaleformMovieFunctionVoid();
            }
            else if(countdownSeconds == 0)
            {
                // Set the countdown time
                RAGE.Game.Graphics.PushScaleformMovieFunction(countdownScaleform, "FADE_MP");
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterString("GO!");
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(255);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(255);
                RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(255);
                RAGE.Game.Graphics.PopScaleformMovieFunctionVoid();

                // Let the player drive
                ToggleStartupFreeze(false);
            }
            else
            {
                // Disable the countdown
                RAGE.Game.Graphics.SetScaleformMovieAsNoLongerNeeded(ref countdownScaleform);
            }
        }

        private void TickEvent(List<Events.TickNametagData> nametags)
        {
            if (countdownSeconds < 0) return;

            // Show the countdown
            RAGE.Game.Graphics.DrawScaleformMovieFullscreen(countdownScaleform, 255, 255, 255, 255, 255);
        }

        private void ToggleStartupFreeze(bool freeze)
        {
            // Toggle the player freeze
            Player.LocalPlayer.FreezePosition(freeze);

            if (Player.LocalPlayer.Vehicle != null && Player.LocalPlayer.Vehicle.Exists)
            {
                // Toggle the vehicle freeze
                Player.LocalPlayer.Vehicle.FreezePosition(freeze);
            }
        }
    }
}
