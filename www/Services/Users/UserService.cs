using System.Threading.Tasks;
using www.DataAccess;
using www.Models;
using www.Cryptography;

namespace www.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly ICryptoProvider cryptoProvider;

        public UserService(IUserRepository userRepository, ICryptoProvider cryptoProvider)
        {
            this.userRepository = userRepository;
            this.cryptoProvider = cryptoProvider;
        }

        public async Task<Result<User>> RegisterAsync(string login, string password, string name, string surname, int age, Genders gender, string interests, string city)
        {
            var user = await userRepository.GetUserAsync(login);
            if (user != null)
                return Result<User>.ErrorResult($"Пользователь с логином \"{login}\" уже существует");

            var passwordHash = cryptoProvider.GeneratePasswordHash(password);
            user = new()
            {
                Login = login,
                PasswordHash = passwordHash,
                Name = name,
                Surname = surname,
                Age = age,
                Gender = gender,
                Interest = interests,
                City = city
            };

            var userAdded = await userRepository.AddUserAsync(user);
            if (!userAdded)
                return Result<User>.ErrorResult("Регистрация завершилась неуспешно");

            return Result<User>.SuccessResult(user);
        }

        public async Task<Result<User>> LoginAsync(string login, string password)
        {
            var user = await userRepository.GetUserAsync(login);
            if (user != null)
            {
                if (cryptoProvider.VerifyPassword(password, user.PasswordHash))
                    return Result<User>.SuccessResult(user);
            }

            return await Task.FromResult(Result<User>.ErrorResult("Неверный логин или пароль"));
        }

        public Task<User> GetUserAsync(int id) => userRepository.GetUserAsync(id);

        public Task<User> GetUserAsync(string login) => userRepository.GetUserAsync(login);

        public Task<User[]> GetUsersAsync() => userRepository.GetUsersAsync();

        public Task<User[]> GetFriendsAsync(int id) => userRepository.GetFriendsAsync(id);
    }
}
