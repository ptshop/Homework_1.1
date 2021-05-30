using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using www.Models;
using www.Pages.Account.Extensions;
using www.Services.Users;
using www.ViewModels;

namespace www.Pages.Account
{
    [Authorize]
    public class FriendsModel : PageModel
    {
        public User[] Friends { get; set; }

        public PageViewModel PageViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync([FromServices] IUserService userService, [FromQuery(Name = "p")] int page = 1)
        {
            const int pageSize = 20;

            var currentUserId = HttpContext.GetCurrentUserId();

            var skip = (page - 1) * pageSize;
            var (friends, totalCount) = await userService.GetFriendsAsync(currentUserId, skip, pageSize);

            Friends = friends;
            PageViewModel = new PageViewModel(totalCount, page, pageSize);

            return Page();
        }
    }
}
