using www.Models;

namespace www.Pages.Account.Extensions
{
    public static class GendersExtensions
    {
        public static string Display(this Genders gender) => gender switch
        {
            Genders.Male => "мужской",
            Genders.Female => "женский",
            _ => ""
        };
    }
}
