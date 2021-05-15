using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using www.Models;
using www.Pages.Account.Extensions;
using www.Services.Users;

namespace www.Pages.Account
{
    [BindProperties]
    [AutoValidateAntiforgeryToken]
    public class RegisterModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "укажите имя")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "укажите фамилию")]
        public string Surname { get; set; }

        [Display(Name = "Возраст")]
        [Required(ErrorMessage = "укажите возраст")]
        [Range(1, 150, ErrorMessage = "значения от 1 до 150")]
        public int Age { get; set; } = 18;

        [Display(Name = "Пол")]
        [Required(ErrorMessage = "укажите пол")]
        public Genders? Gender { get; set; }

        [Display(Name = "Интересы")]
        public string Interest { get; set; }

        [Display(Name = "Город")]
        public string City { get; set; }

        [Display(Name = "Логин")]
        [Required(ErrorMessage = "укажите логин")]
        public string Login { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "укажите пароль")]
        [MinLength(6, ErrorMessage = "минимум 6 символов")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Пароль еще раз")]
        [Compare(nameof(Password), ErrorMessage = "пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        public async Task<IActionResult> OnPostAsync([FromServices] IUserService userService)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.RegisterAsync(Login, Password, Name, Surname, Age, Gender.Value, Interest, City);
                if (result.Success)
                {
                    await HttpContext.SignInCookieAsync(result.Value);

                    if (!string.IsNullOrEmpty(ReturnUrl))
                        return Redirect(ReturnUrl);

                    return RedirectToPage("/Index");
                }

                ModelState.AddModelError(string.Empty, result.Error);
            }

            return Page();
        }
    }
}
