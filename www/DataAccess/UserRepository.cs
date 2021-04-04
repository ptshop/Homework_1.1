using System.Collections.Generic;
using System.Threading.Tasks;
using www.Models;

namespace www.DataAccess
{
    public class UserRepository : IUserRepository
    {
        private static readonly List<User> users = new();

        public async Task<User> FindAsync(string login)
        {
            var user = users.Find(u => u.Login == login);
            return await Task.FromResult(user);
        }

        public async Task<bool> AddAsync(User user)
        {
            users.Add(user);
            return await Task.FromResult(true);
        }
    }
}
