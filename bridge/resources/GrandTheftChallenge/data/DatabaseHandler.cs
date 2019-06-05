using GTANetworkAPI;

namespace GrandTheftChallenge.Data
{
    public class DatabaseHandler : Script
    {
        private static string connectionHandle;

        public DatabaseHandler()
        {
            // Get the database connection settings
            string host = NAPI.Resource.GetSetting<string>(this, "host");
            string user = NAPI.Resource.GetSetting<string>(this, "username");
            string pass = NAPI.Resource.GetSetting<string>(this, "password");
            string db = NAPI.Resource.GetSetting<string>(this, "database");
            string ssl = NAPI.Resource.GetSetting<string>(this, "ssl");

            // Create the connection handle
            connectionHandle = "SERVER=" + host + "; DATABASE=" + db + "; UID=" + user + "; PASSWORD=" + pass + "; SSLMODE=" + ssl + ";";

            // Load all the database stuff
            InitializeStartupModels();
        }

        private void InitializeStartupModels()
        {
            // Load the startup data and store into lists
        }
    }
}
