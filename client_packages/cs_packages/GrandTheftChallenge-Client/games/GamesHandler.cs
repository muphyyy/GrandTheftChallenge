using System;
using System.Collections.Generic;
using GrandTheftChallenge_Client.Data;
using Newtonsoft.Json;
using RAGE;
using RAGE.Elements;

namespace GrandTheftChallenge_Client.Games
{
    public class GamesHandler : Events.Script
    {
        private int countdownSeconds = 0;
        private int countdownScaleform = -1;
        private static List<int> mapObjects = null;

        public GamesHandler()
        {
            Events.Add("LoadTrack", LoadTrackEvent);
            Events.Add("ShowCountdown", ShowCountdownEvent);

            // Add default RAGE's events
            Events.Tick += TickEvent;

            // Get the scaleform for the countdown
            countdownScaleform = RAGE.Game.Graphics.RequestScaleformMovie("countdown");

            // Initialize the list with the map
            mapObjects = new List<int>();
        }

        private void LoadTrackEvent(object[] args)
        {
            // Get the lobby identifier
            uint lobbyId = Convert.ToUInt32(args[0]);

            // Get the track objects
            List<MapModel> map = JsonConvert.DeserializeObject<List<MapModel>>(args[1].ToString());

            foreach(MapModel mapElement in map)
            {
                // Create the object
                MapObject elementHandle = new MapObject(mapElement.ObjectModel, mapElement.Position, mapElement.Rotation, 255, lobbyId);

                // Add the handle to the list
                mapObjects.Add(elementHandle.Handle);
            }

            // Tell the server that the player has finished loading the map
            Events.CallRemote("PlayerLoadedMap");
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
