using System.Security.Claims;

namespace BlazingBlog.Utils
{
    public static class Extensions
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
    }
}
