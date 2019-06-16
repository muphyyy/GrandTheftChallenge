using GrandTheftChallenge.Data;
using GrandTheftChallenge.Data.Model;
using GrandTheftChallenge.Utility;
using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GrandTheftChallenge.Games
{
    public class GamesHandler : Script
    {
        private static Timer countdown = null;

        public static List<GameModel> gamesList;
        public static List<LobbyModel> lobbyList;

        public GamesHandler()
        {
            // Initialize the lobbies and games
            gamesList = new List<GameModel>();
            lobbyList = new List<LobbyModel>();
        }

        public static async void LoadSpawnPoints(LobbyModel lobby)
        {
            // Load the items for the selected track
            List<SpawnPointModel> spawnPoints = await DatabaseHandler.LoadTrackSpawns(lobby.Track).ConfigureAwait(false);

            // Initialize the random seems
            Random random = new Random();

            foreach(Client player in lobby.Players)
            {
                // Get a random spawn point
                SpawnPointModel spawn = spawnPoints[random.Next(spawnPoints.Count)];

                // Set the player into the spawn
                player.Position = spawn.Position;
                player.Rotation = spawn.Rotation;
                player.Dimension = (uint)lobby.Id;

                // Remove the spawn point
                spawnPoints.Remove(spawn);
            }
        }

        public static async void LoadTrackForPlayers(LobbyModel lobby)
        {
            // Load the items for the selected track
            List<MapModel> trackObjects = await DatabaseHandler.LoadTrackMap(lobby.Track).ConfigureAwait(false);

            if(trackObjects.Count > 0)
            {
                // Get the JSON string of the objects
                string trackObjectsJson = NAPI.Util.ToJson(trackObjects);

                // Load the track for all the players
                lobby.Players.ForEach(p => p.TriggerEvent("LoadTrack", lobby.Id, trackObjectsJson));
            }
        }

        public static void StartCountdown(int lobby)
        {
            // Add the countdown for the lobby
            LobbyModel lobbyModel = lobbyList[lobby];
            lobbyModel.Countdown = Constants.COUNTDOWN_TIME;

            // Start the countdown timer
            countdown = new Timer(CountdownTimer, lobby, 0, 1000);
        }

        [RemoteEvent("PlayerLoadedMap")]
        public void PlayerLoadedMapEvent(Client player)
        {
            // Get the lobby where the player is and add a player to the count
            LobbyModel lobby = lobbyList.First(l => l.Players.Contains(player));
            lobby.PlayersReady++;

            if(lobby.PlayersReady == lobby.Players.Count)
            {
                // Start the game
                lobby.PlayersReady = 0;
                StartCountdown(lobby.Id);
            }
        }

        private static void CountdownTimer(object parameter)
        {
            // Get the lobby from the identifier
            int lobbyId = Convert.ToInt32(parameter);
            LobbyModel lobbyModel = lobbyList[lobbyId];

            if(lobbyModel.Countdown != -2)
            {
                // Check if it's the first launch
                bool justJoined = lobbyModel.Countdown == Constants.COUNTDOWN_TIME;

                // Show the counter for the players
                lobbyModel.Players.ForEach(c => c.TriggerEvent("ShowCountdown", lobbyModel.Countdown, justJoined));
                lobbyModel.Countdown--;

                return;
            }

            // Start the game
            countdown.Dispose();
            countdown = null;
        }
    }
}
