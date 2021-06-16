using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using www.Models;
using www.Services.Users;
using www.ViewModels;

namespace www.Pages.Account
{
    [Authorize]
    public class UsersModel : PageModel
    {
        public User[] Users { get; set; }

        public PageViewModel PageViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync([FromServices] IUserService userService, [FromQuery(Name = "p")] int pageNumber = 1)
        {
            const int pageSize = 20;

            var skip = (pageNumber - 1) * pageSize;
            var (users, totalCount) = await userService.GetUsersAsync(skip, pageSize);

            Users = users;
            PageViewModel = new PageViewModel(totalCount, pageNumber, pageSize);

            return Page();
        }
    }
}
