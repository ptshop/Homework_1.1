using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using www.Models;

namespace www.DataAccess
{
    public class UserRepository : IUserRepository
    {
        private readonly string connectionString;
        private readonly ILogger<IUserRepository> logger;

        public UserRepository(string connectionString, ILogger<IUserRepository> logger)
        {
            this.connectionString = connectionString;
            this.logger = logger;
        }

        public async Task<User> GetUserAsync(int id)
        {
            try
            {
                var query = "SELECT * FROM Users WHERE Id = @Id;";

                await using var connection = new MySqlConnection(connectionString);

                await using var command = new MySqlCommand(query, connection);
                command.Parameters.Add("Id", MySqlDbType.Int32).Value = id;

                await connection.OpenAsync();

                await using var dataReader = (MySqlDataReader)await command.ExecuteReaderAsync();
                if (await dataReader.ReadAsync())
                {
                    return new User()
                    {
                        Id = dataReader.GetInt32("Id"),
                        Login = dataReader.GetString("Login"),
                        PasswordHash = dataReader.GetString("PasswordHash"),
                        Name = dataReader.GetString("Name"),
                        Surname = dataReader.GetString("Surname"),
                        Age = dataReader.GetInt32("Age"),
                        Gender = (Genders)dataReader.GetInt32("Gender"),
                        Interest = !dataReader.IsDBNull(dataReader.GetOrdinal("Interest")) ? dataReader.GetString("Interest") : null,
                        City = !dataReader.IsDBNull(dataReader.GetOrdinal("City")) ? dataReader.GetString("City") : null
                    };
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Get user by ID failed");
            }

            return null;
        }

        public async Task<User> GetUserAsync(string login)
        {
            try
            {
                var query = "SELECT * FROM Users WHERE Login = @Login;";

                await using var connection = new MySqlConnection(connectionString);

                await using var command = new MySqlCommand(query, connection);
                command.Parameters.Add("Login", MySqlDbType.String).Value = login;

                await connection.OpenAsync();

                await using var dataReader = (MySqlDataReader)await command.ExecuteReaderAsync();
                if (await dataReader.ReadAsync())
                {
                    return new User()
                    {
                        Id = dataReader.GetInt32("Id"),
                        Login = dataReader.GetString("Login"),
                        PasswordHash = dataReader.GetString("PasswordHash"),
                        Name = dataReader.GetString("Name"),
                        Surname = dataReader.GetString("Surname"),
                        Age = dataReader.GetInt32("Age"),
                        Gender = (Genders)dataReader.GetInt32("Gender"),
                        Interest = !dataReader.IsDBNull(dataReader.GetOrdinal("Interest")) ? dataReader.GetString("Interest") : null,
                        City = !dataReader.IsDBNull(dataReader.GetOrdinal("City")) ? dataReader.GetString("City") : null
                    };
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Get user by login failed");
            }

            return null;
        }

        public async Task<bool> AddUserAsync(User user)
        {
            try
            {
                var query =
@"
INSERT INTO Users (Login, PasswordHash, Name, Surname, Age, Gender, Interest, City)
VALUES (@Login, @PasswordHash, @Name, @Surname, @Age, @Gender, @Interest, @City);
";
                await using var connection = new MySqlConnection(connectionString);

                await using var command = new MySqlCommand(query, connection);
                command.Parameters.Add("Login", MySqlDbType.String).Value = user.Login;
                command.Parameters.Add("PasswordHash", MySqlDbType.String).Value = user.PasswordHash;
                command.Parameters.Add("Name", MySqlDbType.String).Value = user.Name;
                command.Parameters.Add("Surname", MySqlDbType.String).Value = user.Surname;
                command.Parameters.Add("Age", MySqlDbType.UInt32).Value = user.Age;
                command.Parameters.Add("Gender", MySqlDbType.UInt16).Value = (int)user.Gender;
                command.Parameters.Add("Interest", MySqlDbType.String).Value = user.Interest;
                command.Parameters.Add("City", MySqlDbType.String).Value = user.City;

                await connection.OpenAsync();

                return await command.ExecuteNonQueryAsync() > 0;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Add user failed");

                return false;
            }
        }

        public async Task<User[]> GetUsersAsync()
        {
            try
            {
                var query = "SELECT Id, Name, Surname FROM Users;";
                await using var connection = new MySqlConnection(connectionString);

                await using var command = new MySqlCommand(query, connection);

                await connection.OpenAsync();

                var users = new List<User>();
                await using var dataReader = (MySqlDataReader)await command.ExecuteReaderAsync();
                while (await dataReader.ReadAsync())
                {
                    users.Add(new User()
                    {
                        Id = dataReader.GetInt32("Id"),
                        Name = dataReader.GetString("Name"),
                        Surname = dataReader.GetString("Surname")
                    });
                }

                return users.ToArray();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Get users failed");
            }

            return Array.Empty<User>();
        }

        public async Task<User[]> GetFriendsAsync(int id)
        {
            try
            {
                var query =
@"
SELECT u.Id, u.Name, u.Surname
  FROM Users u
  JOIN UsersToUsers utu ON
    utu.UserId = @Id  AND utu.FriendId = u.Id OR
    utu.UserId = u.Id AND utu.FriendId = @Id 
  WHERE
    utu.UserId = @Id OR utu.FriendId = @Id;
";

                await using var connection = new MySqlConnection(connectionString);

                await using var command = new MySqlCommand(query, connection);
                command.Parameters.Add("Id", MySqlDbType.Int32).Value = id;

                await connection.OpenAsync();

                var users = new List<User>();
                await using var dataReader = (MySqlDataReader)await command.ExecuteReaderAsync();
                while (await dataReader.ReadAsync())
                {
                    users.Add(new User()
                    {
                        Id = dataReader.GetInt32("Id"),
                        Name = dataReader.GetString("Name"),
                        Surname = dataReader.GetString("Surname")
                    });
                }

                return users.ToArray();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Get friends failed");
            }

            return Array.Empty<User>();
        }
        
        public async Task<bool> UsersAreFriendsAsync(int id1, int id2)
        {
            try
            {
                var query =
@"
SELECT EXISTS
(
  SELECT 1 FROM UsersToUsers
  WHERE
    UserId = @Id1 AND FriendId = @Id2 OR
    UserId = @Id2 AND FriendId = @Id1
);
";
                await using var connection = new MySqlConnection(connectionString);

                await using var command = new MySqlCommand(query, connection);
                command.Parameters.Add("Id1", MySqlDbType.Int32).Value = id1;
                command.Parameters.Add("Id2", MySqlDbType.Int32).Value = id2;

                await connection.OpenAsync();

                await using var dataReader = (MySqlDataReader)await command.ExecuteReaderAsync();
                if (await dataReader.ReadAsync())
                {
                    return dataReader.GetInt32(0) != 0;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Check users are friends failed");
            }

            return false;
        }

        public async Task<bool> MakeFriendsAsync(int userId, int friendId)
        {
            try
            {
                var query =
@"
INSERT INTO UsersToUsers (UserId, FriendId)
VALUES (@UserId, @FriendId);
";
                await using var connection = new MySqlConnection(connectionString);

                await using var command = new MySqlCommand(query, connection);
                command.Parameters.Add("UserId", MySqlDbType.Int32).Value = userId;
                command.Parameters.Add("FriendId", MySqlDbType.Int32).Value = friendId;

                await connection.OpenAsync();

                return await command.ExecuteNonQueryAsync() > 0;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Make users friends failed");

                return false;
            }
        }
    }
}
