using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using www.Models;

namespace www.Services.Users
{
    public class UserService : IUserService
    {
        private static List<User> _users = new();

        public async Task<Result> RegisterAsync(string login, string password, string name, string surname, int age, Genders gender, string interests, string city)
        {
            if (_users.Any(u => u.Login == login))
                return Result.ErrorResult($"Пользователь с логином \"{login}\" уже существует");

            var user = new User
            {
                Login = login,
                Password = password,
                Name = name,
                Surname = surname,
                Age = age,
                Gender = gender,
                Interest = interests,
                City = city
            };

            _users.Add(user);

            return await Task.FromResult(Result.SuccessResult);
        }

        public async Task<Result> LoginAsync(string login, string password)
        {
            if (_users.Any(u => u.Login == login && u.Password == password))
            {
                return Result.SuccessResult;
            }

            return await Task.FromResult(Result.ErrorResult("Неверный логин или пароль"));
        }
    }
}
