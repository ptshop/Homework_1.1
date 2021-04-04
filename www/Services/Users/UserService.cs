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

        public async Task<Result> RegisterAsync(string login, string password, string name, string surname, int age, Genders gender, string interests, string city)
        {
            var user = await userRepository.FindAsync(login);
            if (user != null)
                return Result.ErrorResult($"Пользователь с логином \"{login}\" уже существует");

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
                return Result.ErrorResult("Регистрация завершилась неуспешно");

            return Result.SuccessResult;
        }

        public async Task<Result> LoginAsync(string login, string password)
        {
            var user = await userRepository.FindAsync(login);
            if (user != null)
            {
                if (cryptoProvider.VerifyPassword(password, user.PasswordHash))
                    return Result.SuccessResult;
            }

            return await Task.FromResult(Result.ErrorResult("Неверный логин или пароль"));
        }
    }
}
