using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using www.Models;
using www.Pages.Account.Extensions;
using www.Services.Users;

namespace www.Pages.Account
{
    [Authorize]
    public class FriendsModel : PageModel
    {
        public User[] Friends { get; set; }

        public async Task<IActionResult> OnGetAsync([FromServices] IUserService userService)
        {
            var currentUserId = HttpContext.GetCurrentUserId();

            Friends = await userService.GetFriendsAsync(currentUserId);

            return Page();
        }
    }
}
