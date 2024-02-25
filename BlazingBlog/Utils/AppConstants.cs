using Microsoft.AspNetCore.Components.Sections;

namespace BlazingBlog.Utils
{
    public static class AppConstants
    {
        public const string FullName = "FullName";
        public const string LoggedInAt = "LoggedInAt";

        public static SectionContent HeaderSection { get; private set; } = new();
    }
}
