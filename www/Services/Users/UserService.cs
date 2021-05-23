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
                return Result<User>.FailedResult($"Пользователь с логином \"{login}\" уже существует");

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

            // TODO: Не ставится Id пользователя

            var userAdded = await userRepository.AddUserAsync(user);
            if (!userAdded)
                return Result<User>.FailedResult("Регистрация завершилась неуспешно");

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

            return Result<User>.FailedResult("Неверный логин или пароль");
        }

        public Task<User> GetUserAsync(int id) => userRepository.GetUserAsync(id);

        public Task<User> GetUserAsync(string login) => userRepository.GetUserAsync(login);

        public Task<User[]> GetUsersAsync() => userRepository.GetUsersAsync();

        public Task<User[]> GetFriendsAsync(int id) => userRepository.GetFriendsAsync(id);
        
        public Task<bool> UsersAreFriendsAsync(int id1, int id2) => userRepository.UsersAreFriendsAsync(id1, id2);

        public async Task<Result> MakeFriendsAsync(int userId, int friendId)
        {
            if (userId == friendId)
                return Result.FailedResult("Вы не можете добавить себя в друзья");

            var user = await userRepository.GetUserAsync(userId);
            if (user == null)
                return Result.FailedResult("Пользователь не найден");

            var friend = await userRepository.GetUserAsync(friendId);
            if (friend == null)
                return Result.FailedResult("Пользователь не найден");

            if (await userRepository.UsersAreFriendsAsync(userId, friendId))
                return Result.FailedResult($"Вы уже дружите с {friend.Name} {friend.Surname}");

            if (!await userRepository.MakeFriendsAsync(userId, friendId))
                return Result.FailedResult($"Не удалось добавить пользователя {friend.Name} {friend.Surname} в друзья");

            return Result.SuccessResult;
        }
    }
}
