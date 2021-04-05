using System.Threading.Tasks;
using www.Models;

namespace www.DataAccess
{
    public interface IUserRepository
    {
        Task<User> FindAsync(int id);
        Task<User> FindAsync(string login);
        Task<bool> AddAsync(User user);
    }
}
