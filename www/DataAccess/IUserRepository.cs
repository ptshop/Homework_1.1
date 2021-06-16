using System.Threading.Tasks;
using www.Models;

namespace www.DataAccess
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(int id);
        Task<User> GetUserAsync(string login);
        Task<bool> AddUserAsync(User user);
        Task<(User[], int)> GetUsersAsync(int skip, int take);
        Task<(User[], int)> GetFriendsAsync(int id, int skip, int take);
        Task<bool> UsersAreFriendsAsync(int id1, int id2);
        Task<bool> MakeFriendsAsync(int userId, int friendId);
    }
}
