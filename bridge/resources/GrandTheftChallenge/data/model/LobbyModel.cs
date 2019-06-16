using GTANetworkAPI;
using System.Collections.Generic;

namespace GrandTheftChallenge.Data.Model
{
    public class LobbyModel
    {
        public int Id { get; set; }
        public int Track { get; set; }
        public List<Client> Players { get; set; }
        public int PlayersReady { get; set; }
        public int Countdown { get; set; }
    }
}
