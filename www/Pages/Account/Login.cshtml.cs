using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using www.Pages.Account.Extensions;
using www.Services.Users;

namespace www.Pages.Account
{
    [BindProperties]
    [AutoValidateAntiforgeryToken]
    public class LoginModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Укажите логин")]
        public string Login { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Укажите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync([FromServices] IUserService userService)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.LoginAsync(Login, Password);
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
