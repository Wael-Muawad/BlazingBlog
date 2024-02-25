using System.ComponentModel.DataAnnotations;

namespace BlazingBlog.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(75)]
        public string Slug { get; set; }

        public bool ShowOnNavbar { get; set; }

        public Category Clone() => (Category)MemberwiseClone();

        public static Category[] GetSeedCategories()
        {
            return [
                new Category{Name = "C#", Slug = "c-sharp", ShowOnNavbar = true},
                new Category{Name = "ASP.NET Core", Slug = "asp-net-core", ShowOnNavbar = true},
                new Category{Name = "Blazor", Slug = "blazor", ShowOnNavbar = true},
                new Category{Name = "Entity Framework", Slug = "entity-framework", ShowOnNavbar = false},
                new Category{Name = "MVC", Slug = "mvc", ShowOnNavbar = true},
                new Category{Name = "Razor Pages", Slug = "razor-pages", ShowOnNavbar = false},
                new Category{Name = "Web API", Slug = "web-api", ShowOnNavbar = false},
                new Category{Name = "Xamarin", Slug = "xamarin", ShowOnNavbar = true},
                new Category{Name = "WPF", Slug = "wpf", ShowOnNavbar = false},
                new Category{Name = "WinForms", Slug = "winforms", ShowOnNavbar = false},
                new Category{Name = "Azure", Slug = "azure", ShowOnNavbar = false},
                new Category{Name = "SQL Server", Slug = "sql-server", ShowOnNavbar = false},
                new Category{Name = "GraphQL", Slug = "graphql", ShowOnNavbar = true}
                ];
        }
    }
}
