using BlazingBlog.Data;
using BlazingBlog.Data.Entities;
using BlazingBlog.Utils;
using Microsoft.EntityFrameworkCore;

namespace BlazingBlog.Services.CategoriesService
{
    public class CategoryService : ICategoryService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public CategoryService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory=contextFactory;
        }



        //using the template pattern
        private async Task<TResult> ExecuteOnContext<TResult>(Func<ApplicationDbContext, Task<TResult>> contextFactory)
        {
            using var context = _contextFactory.CreateDbContext();
            return await contextFactory.Invoke(context);
        }


        public async Task<Category[]> GetCategories()
        {
            return await ExecuteOnContext(async context =>
            {
                var categories = await context.Categories.AsNoTracking().ToArrayAsync();
                return categories;
            });
        }


        public async Task<Category> SaveCategory(Category category)
        {
            return await ExecuteOnContext(async context =>
            {
                if (category.Id == 0)
                {
                    if (await context.Categories
                        .AsNoTracking()
                        .AnyAsync(c => c.Name == category.Name))
                    {
                        throw new InvalidOperationException($"Category with name {category.Name} already exist");
                    }
                    category.Slug = category.Name.ToSlug();
                    await context.Categories.AddAsync(category);
                }
                else
                {
                    if (await context.Categories
                        .AsNoTracking()
                        .AnyAsync(c => c.Name == category.Name))
                    {
                        throw new InvalidOperationException($"Category with name {category.Name} already exist");
                    }

                    var dbCategory = await context.Categories.FindAsync(category.Id);

                    dbCategory.Name = category.Name;
                    dbCategory.ShowOnNavbar = category.ShowOnNavbar;
                    dbCategory.Slug = category.Name.ToSlug();
                }

                await context.SaveChangesAsync();
                return category;
            });
        }


        public async Task<Category?> GetCategoryBySlug(string slug) =>
            await ExecuteOnContext(
                    async context =>
                    await context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Slug == slug)
                );
    }
}
