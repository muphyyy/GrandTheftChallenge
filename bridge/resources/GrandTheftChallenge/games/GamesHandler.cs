using GrandTheftChallenge.Data.Model;
using GrandTheftChallenge.Utility;
using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Threading;

namespace GrandTheftChallenge.Games
{
    public class GamesHandler : Script
    {
        private static Timer countdown = null;

        public static List<LobbyModel> lobbyList;

        public GamesHandler()
        {
            // Initialize the lobbies
            lobbyList = new List<LobbyModel>();
        }

        public static void StartCountdown(int lobby)
        {
            // Add the countdown for the lobby
            LobbyModel lobbyModel = lobbyList[lobby];
            lobbyModel.Countdown = Constants.COUNTDOWN_TIME;

            // Start the countdown timer
            countdown = new Timer(CountdownTimer, lobby, 0, 1000);
        }

        private static void CountdownTimer(object parameter)
        {
            // Get the lobby from the identifier
            int lobbyId = Convert.ToInt32(parameter);
            LobbyModel lobbyModel = lobbyList[lobbyId];

            if(lobbyModel.Countdown != -1)
            {
                // Show the counter for the players
                lobbyModel.Players.ForEach(c => c.TriggerEvent("ShowCountdown", lobbyModel.Countdown));
                lobbyModel.Countdown--;
            }
            else
            {
                // Start the game
            }

            // Show the countdown for all the players

        }
    }
}
