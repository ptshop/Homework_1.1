using System.ComponentModel.DataAnnotations;
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
    public class ProfileModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Display(Name = "Возраст")]
        public int Age { get; set; }

        [Display(Name = "Пол")]
        public Genders Gender { get; set; }

        [Display(Name = "Интересы")]
        public string Interest { get; set; }

        [Display(Name = "Город")]
        public string City { get; set; }

        public bool IsFriend { get; private set; }

        public bool CanAddToFriends { get; private set; }

        public async Task<IActionResult> OnGetAsync([FromServices] IUserService userService)
        {
            var currentUserId = HttpContext.GetCurrentUserId();

            if (Id == 0 && currentUserId != 0)
            {
                return RedirectToPage("Profile", new { id = currentUserId });
            }

            var user = await userService.GetUserAsync(Id);
            if (user == null)
                return NotFound();

            Id = user.Id;
            Name = user.Name;
            Surname = user.Surname;
            Age = user.Age;
            Gender = user.Gender;
            Interest = user.Interest;
            City = user.City;

            if (user.Id == currentUserId)
            {
                IsFriend = false;
                CanAddToFriends = false;
            }
            else
            {
                IsFriend = await userService.UsersAreFriendsAsync(user.Id, currentUserId);
                CanAddToFriends = !IsFriend;
            }

            return Page();
        }

        public async Task<IActionResult> OnGetAddToFriendsAsync([FromServices] IUserService userService)
        {
            var currentUserId = HttpContext.GetCurrentUserId();

            var (success, error) = await userService.MakeFriendsAsync(currentUserId, Id);
            return new JsonResult(new { success, error });
        }
    }
}
