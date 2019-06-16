using GrandTheftChallenge.Data.Model;
using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace GrandTheftChallenge.Data
{
    public class DatabaseHandler : Script
    {
        private static string connectionHandle;

        [ServerEvent(Event.ResourceStart)]
        public void ResourceStartEvent()
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

        public static async Task<int> RegisterAccount(string username, string email, string password, string social)
        {
            int registeredAccount = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO `accounts` (`username`, `email`, `password`, `socialName`) VALUES (@username, @email, SHA2(@password, '256'), @social)";
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@social", social);

                try
                {
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                    registeredAccount = (int)command.LastInsertedId;
                }
                catch (Exception ex)
                {
                    NAPI.Util.ConsoleOutput("[EXCEPTION RegisterAccount] " + ex.Message);
                    NAPI.Util.ConsoleOutput("[EXCEPTION RegisterAccount] " + ex.StackTrace);
                }
            }

            return registeredAccount;
        }

        public static async Task<bool> LoginAccount(string username, string password)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT `id` FROM `accounts` WHERE (`username` = @username OR `email` = @username) AND `password` = SHA2(@password, '256') LIMIT 1";
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                return reader.HasRows;
            }
        }

        public static async Task<List<MapModel>> LoadTrackMap(int trackId)
        {
            List<MapModel> map = new List<MapModel>();

            using (MySqlConnection connection = new MySqlConnection(connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT `object`, `posX`, `posY`, `posZ`, `rotX`, `rotY`, `rotZ` FROM `maps` WHERE `trackId` = @trackId";
                command.Parameters.AddWithValue("@trackId", trackId);

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                while(reader.HasRows)
                {
                    // Get the position and rotation
                    float positionX = reader.GetFloat(reader.GetOrdinal("posX"));
                    float positionY = reader.GetFloat(reader.GetOrdinal("posY"));
                    float positionZ = reader.GetFloat(reader.GetOrdinal("posZ"));

                    float rotationX = reader.GetFloat(reader.GetOrdinal("rotX"));
                    float rotationY = reader.GetFloat(reader.GetOrdinal("rotY"));
                    float rotationZ = reader.GetFloat(reader.GetOrdinal("rotZ"));


                    MapModel mapElement = new MapModel()
                    {
                        ObjectModel = reader.GetInt32(reader.GetOrdinal("object")),
                        Position = new Vector3(positionX, positionY, positionZ),
                        Rotation = new Vector3(rotationX, rotationY, rotationZ)
                    };

                    map.Add(mapElement);
                }
            }

            return map;
        }

        public static async Task<List<SpawnPointModel>> LoadTrackSpawns(int trackId)
        {
            List<SpawnPointModel> spawns = new List<SpawnPointModel>();

            using (MySqlConnection connection = new MySqlConnection(connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT `posX`, `posY`, `posZ`, `rotX`, `rotY`, `rotZ` FROM `spawns` WHERE `trackId` = @trackId";
                command.Parameters.AddWithValue("@trackId", trackId);

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                while (reader.HasRows)
                {
                    // Get the position and rotation
                    float positionX = reader.GetFloat(reader.GetOrdinal("posX"));
                    float positionY = reader.GetFloat(reader.GetOrdinal("posY"));
                    float positionZ = reader.GetFloat(reader.GetOrdinal("posZ"));

                    float rotationX = reader.GetFloat(reader.GetOrdinal("rotX"));
                    float rotationY = reader.GetFloat(reader.GetOrdinal("rotY"));
                    float rotationZ = reader.GetFloat(reader.GetOrdinal("rotZ"));


                    SpawnPointModel spawnPoint = new SpawnPointModel()
                    {
                        Position = new Vector3(positionX, positionY, positionZ),
                        Rotation = new Vector3(rotationX, rotationY, rotationZ)
                    };

                    spawns.Add(spawnPoint);
                }
            }

            return spawns;
        }
    }
}
