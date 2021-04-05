using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using www.Models;
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

        public async Task<IActionResult> OnGetAsync([FromServices] IUserService userService)
        {
            var user = await userService.FindUserAsync(Id);
            if (user == null)
                return NotFound();

            Id = user.Id;
            Name = user.Name;
            Surname = user.Surname;
            Age = user.Age;
            Gender = user.Gender;
            Interest = user.Interest;
            City = user.City;

            return Page();
        }
    }
}
