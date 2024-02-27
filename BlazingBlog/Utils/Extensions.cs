using System.Security.Claims;
using System.Text.RegularExpressions;

namespace BlazingBlog.Utils
{
    public static partial class Extensions
    {

        public static string GetFullNameClaim(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(AppConstants.FullName) ?? string.Empty;
        }

        public static string GetLoggedInAtClaim(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(AppConstants.LoggedInAt) ?? string.Empty;
        }

        public static string GetIdClaim(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        public static string ToSlug(this string text)
        {
            // "Blazor (WASM) -> blazor--wasm-
            text = SlugRegex().Replace(text.ToLowerInvariant(), "-");

            return text.Replace("--", "-") // blazor-wasm-
                       .Trim('-'); // blazor-wasm
        }

        [GeneratedRegex(@"[^0-9a-z_]")]
        private static partial Regex SlugRegex();


        public static string TimeDisplaied(this DateTime? dateTime)
        {
            return dateTime?.ToString("MMM dd") ?? string.Empty;
        }
    }
}
