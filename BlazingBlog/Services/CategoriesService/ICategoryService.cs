using BlazingBlog.Data;
using BlazingBlog.Data.Entities;

namespace BlazingBlog.Services.CategoriesService
{
    public interface ICategoryService
    {
        Task<Category[]> GetCategories();
        Task<Category?> GetCategoryBySlug(string slug);
        Task<Category> SaveCategory(Category category);
    }
}