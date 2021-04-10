using System.Threading.Tasks;
using www.Models;

namespace www.DataAccess
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(int id);
        Task<User> GetUserAsync(string login);
        Task<bool> AddUserAsync(User user);
        Task<User[]> GetUsersAsync();
        Task<User[]> GetFriendsAsync(int id);
    }
}
