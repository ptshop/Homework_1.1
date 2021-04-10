using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using www.Models;

namespace www.DataAccess
{
    public class UserRepository : IUserRepository
    {
        private readonly string connectionString;

        public UserRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<User> GetUserAsync(int id)
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

            return null;
        }

        public async Task<User> GetUserAsync(string login)
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

            return null;
        }

        public async Task<bool> AddUserAsync(User user)
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

            try
            {
                return await command.ExecuteNonQueryAsync() > 0;
            }
            catch
            {
                // TODO: log
                return false;
            }
        }

        public async Task<User[]> GetUsersAsync()
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

        public async Task<User[]> GetFriendsAsync(int id)
        {
            var query =
@"
SELECT Users.Id, Users.Name, Users.Surname
    FROM Users
    JOIN UsersToUsers ON UsersToUsers.FriendId = Users.Id
    WHERE UsersToUsers.UserId = @Id
UNION
SELECT Users.Id, Users.Name, Users.Surname
    FROM Users
    JOIN UsersToUsers ON UsersToUsers.UserId = Users.Id
    WHERE UsersToUsers.FriendId = @Id
ORDER BY Id;
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
    }
}
