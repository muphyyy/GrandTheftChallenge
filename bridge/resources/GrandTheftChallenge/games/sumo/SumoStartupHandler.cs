using GrandTheftChallenge.Data.Model;
using GrandTheftChallenge.Games;
using GTANetworkAPI;

namespace GrandTheftChallenge.games.Sumo
{
    public class SumoStartupHandler : Script
    {
        public static void Initialize(LobbyModel lobby)
        {
            // Load all the spawn points and place the players into them
            GamesHandler.LoadSpawnPoints(lobby);

            // Make all the players load the map from the track
            GamesHandler.LoadTrackForPlayers(lobby);
        }
    }
}
