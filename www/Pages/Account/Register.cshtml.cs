using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using www.Models;
using www.Services.Authorization;
using www.Pages.Account.Extensions;

namespace www.Pages.Account
{
    [BindProperties]
    [AutoValidateAntiforgeryToken]
    public class RegisterModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Укажите имя")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Укажите фамилию")]
        public string Surname { get; set; }

        [Display(Name = "Возраст")]
        [Required(ErrorMessage = "Укажите возраст")]
        [Range(1, 150, ErrorMessage = "Значения от 1 до 150")]
        public int Age { get; set; } = 18;

        [Display(Name = "Пол")]
        [Required(ErrorMessage = "Укажите пол")]
        public Genders? Gender { get; set; }

        [Display(Name = "Интересы")]
        public string Interest { get; set; }

        [Display(Name = "Город")]
        public string City { get; set; }

        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Укажите логин")]
        public string Login { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Укажите пароль")]
        [MinLength(6, ErrorMessage = "Минимальная длина 6 символов")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Подтвердите пароль")]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        public async Task<IActionResult> OnPostAsync([FromServices] IAuthorizationService authorizationService)
        {
            if (ModelState.IsValid)
            {
                //await authorizationService.RegisterAsync(Login, Password);
                await HttpContext.SignInCookieAsync(Login);

                if (!string.IsNullOrEmpty(ReturnUrl))
                    return Redirect(ReturnUrl);

                return RedirectToPage("/Index");
            }

            return Page();
        }
    }
}
