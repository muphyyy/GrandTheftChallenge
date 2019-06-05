using GTANetworkAPI;

namespace GrandTheftChallenge.Character
{
    public class Initializer : Script
    {
        [ServerEvent(Event.PlayerConnected)]
        private async void PlayerConnectedAsyncEvent(Client player)
        {
            // Load the account from the player's Social Name
        } 
    }
}
