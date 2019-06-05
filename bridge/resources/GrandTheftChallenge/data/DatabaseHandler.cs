using GrandTheftChallenge.Data.Model;
using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Threading.Tasks;

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

        public static async Task<AccountModel> GetPlayerAccount(string socialName)
        {
            AccountModel account = null;

            using (MySqlConnection connection = new MySqlConnection(connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT `id`, `state`, `lastLogged` FROM `accounts` WHERE `socialName` = @socialName LIMIT 1";
                command.Parameters.AddWithValue("@socialName", socialName);

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                if (reader.HasRows)
                {
                    await reader.ReadAsync().ConfigureAwait(false);

                    account = new AccountModel();
                    {
                        account.Id = reader.GetInt32(reader.GetOrdinal("id"));
                        account.State = reader.GetInt32(reader.GetOrdinal("state"));
                        account.LastLogged = reader.GetString(reader.GetOrdinal("lastLogged"));
                    }
                }
            }

            return account;
        }

        public static async Task<string> GetAccountBanReason(int accountId)
        {
            string reason = string.Empty;

            using (MySqlConnection connection = new MySqlConnection(connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT `reason` FROM `punishments` WHERE `player` = @player ORDER BY `time` DESC LIMIT 1";
                command.Parameters.AddWithValue("@player", accountId);

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                if (reader.HasRows)
                {
                    await reader.ReadAsync().ConfigureAwait(false);
                    reason = reader.GetString(reader.GetOrdinal("reason"));
                }
            }

            return reason;
        }
    }
}
