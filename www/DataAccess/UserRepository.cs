using System;
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

        public async Task<User> FindAsync(int id)
        {
            await using (MySqlConnection connection = new(connectionString))
            {
                string sql = "SELECT * FROM Users WHERE Id = @Id;";

                await using (MySqlCommand command = new(sql, connection))
                {
                    command.Parameters.Add("Id", MySqlDbType.Int32).Value = id;

                    await connection.OpenAsync();

                    await using (var dataReader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
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
                                City = !dataReader.IsDBNull(dataReader.GetOrdinal("City")) ? dataReader.GetString("City") : null,
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<User> FindAsync(string login)
        {
            await using (MySqlConnection connection = new(connectionString))
            {
                string sql = "SELECT * FROM Users WHERE Login = @Login;";

                await using (MySqlCommand command = new(sql, connection))
                {
                    command.Parameters.Add("Login", MySqlDbType.String).Value = login;

                    await connection.OpenAsync();

                    await using (var dataReader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
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
                                City = !dataReader.IsDBNull(dataReader.GetOrdinal("City")) ? dataReader.GetString("City") : null,
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<bool> AddAsync(User user)
        {
            await using (MySqlConnection connection = new(connectionString))
            {
                string sql = "INSERT INTO Users " +
                    "(Login, PasswordHash, Name, Surname, Age, Gender, Interest, City) " +
                    "VALUES( @Login, @PasswordHash, @Name, @Surname, @Age, @Gender, @Interest, @City);";

                await using (MySqlCommand command = new(sql, connection))
                {
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
                        var t = await command.ExecuteNonQueryAsync();
                        return t > 0;
                    }
                    catch (Exception e)
                    {

                        return false;
                    }

                }
            }
        }
    }
}
