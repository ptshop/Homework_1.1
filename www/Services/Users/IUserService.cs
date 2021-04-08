using System.Threading.Tasks;
using www.Models;

namespace www.Services.Users
{
    public interface IUserService
    {
        Task<Result<User>> RegisterAsync(string login, string password, string name, string surname, int age, Genders gender, string interests, string city);
        Task<Result<User>> LoginAsync(string login, string password);
        Task<User> FindUserAsync(int id);
        Task<User> FindUserAsync(string login);
        Task<User[]> FindFriendsAsync(int id);
    }
}
