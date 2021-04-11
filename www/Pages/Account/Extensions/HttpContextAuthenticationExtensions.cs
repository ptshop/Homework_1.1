using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using www.Models;

namespace www.Pages.Account.Extensions
{
    public static class HttpContextAuthenticationExtensions
    {
        public static async Task SignInCookieAsync(this HttpContext httpContext, User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim("Id", user.Id.ToString())
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        public static async Task SignOutCookieAsync(this HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public static int GetCurrentUserId(this HttpContext httpContext)
        {
            var idString = httpContext.User?.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            if (idString != null && int.TryParse(idString, out var id))
                return id;

            return 0;
        }
    }
}
