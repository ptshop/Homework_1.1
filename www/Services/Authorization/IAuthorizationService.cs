using System.Threading.Tasks;

namespace www.Services.Authorization
{
    public interface IAuthorizationService
    {
        Task<bool> RegisterAsync(string login, string password);
    }
}
