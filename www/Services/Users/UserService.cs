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
            var user = await userRepository.FindAsync(login);
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

            var userAdded = await userRepository.AddAsync(user);
            if (!userAdded)
                return Result<User>.ErrorResult("Регистрация завершилась неуспешно");

            return Result<User>.SuccessResult(user);
        }

        public async Task<Result<User>> LoginAsync(string login, string password)
        {
            var user = await userRepository.FindAsync(login);
            if (user != null)
            {
                if (cryptoProvider.VerifyPassword(password, user.PasswordHash))
                    return Result<User>.SuccessResult(user);
            }

            return await Task.FromResult(Result<User>.ErrorResult("Неверный логин или пароль"));
        }

        public Task<User> FindUserAsync(int id) => userRepository.FindAsync(id);

        public Task<User> FindUserAsync(string login) => userRepository.FindAsync(login);
    }
}
