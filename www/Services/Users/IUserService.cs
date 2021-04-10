using System.Threading.Tasks;
using www.Models;

namespace www.Services.Users
{
    public interface IUserService
    {
        Task<Result<User>> RegisterAsync(string login, string password, string name, string surname, int age, Genders gender, string interests, string city);
        Task<Result<User>> LoginAsync(string login, string password);
        Task<User> GetUserAsync(int id);
        Task<User> GetUserAsync(string login);
        Task<User[]> GetUsersAsync();
        Task<User[]> GetFriendsAsync(int id);
    }
}
