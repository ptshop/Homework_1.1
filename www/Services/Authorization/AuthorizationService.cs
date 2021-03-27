using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace www.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        public async Task<bool> RegisterAsync(string login, string password)
        {
            return await Task.FromResult(true);
        }
    }
}
