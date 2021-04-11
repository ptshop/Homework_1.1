using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using www.Models;
using www.Services.Users;

namespace www.Pages.Account
{
    [Authorize]
    public class UsersModel : PageModel
    {
        public User[] Users { get; set; }

        public async Task<IActionResult> OnGetAsync([FromServices] IUserService userService)
        {
            Users = await userService.GetUsersAsync();

            return Page();
        }
    }
}
