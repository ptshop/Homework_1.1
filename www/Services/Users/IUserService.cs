using System.Threading.Tasks;
using www.Models;

namespace www.Services.Users
{
    public interface IUserService
    {
        Task<Result> RegisterAsync(string login, string password, string name, string surname, int age, Genders gender, string interests, string city);
        Task<Result> LoginAsync(string login, string password);
    }
}
