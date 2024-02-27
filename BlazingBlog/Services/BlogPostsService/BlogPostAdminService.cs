using BlazingBlog.Data;
using BlazingBlog.Data.Entities;
using BlazingBlog.Models;
using BlazingBlog.Utils;
using Microsoft.EntityFrameworkCore;

namespace BlazingBlog.Services.BlogPostsService
{
    public class BlogPostAdminService : IBlogPostAdminService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public BlogPostAdminService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory=contextFactory;
        }



        //using the template pattern
        private async Task<TResult> ExecuteOnContext<TResult>(Func<ApplicationDbContext, Task<TResult>> contextFactory)
        {
            using var context = _contextFactory.CreateDbContext();
            return await contextFactory.Invoke(context);
        }



        public async Task<PagedResult<BlogPost>> GetBlogPosts(int startIndex, int pageSize)
        {
            var result = await ExecuteOnContext(async context =>
             {
                 var query = context.BlogPosts.AsNoTracking();
                 var couunt = await query.CountAsync();

                 var records = await query.Include(b => b.Category)
                                    .OrderByDescending(b => b.Id)
                                    .Skip(startIndex)
                                    .Take(pageSize)
                                    .ToArrayAsync();

                 return new PagedResult<BlogPost>(records, couunt);
             });

            return result;
        }


        public async Task<BlogPost?> GetBlogPostById(int id)
        {
            var result = await ExecuteOnContext(async context =>
            {
                return await context.BlogPosts.AsNoTracking()
                                    .Include(b => b.Category)
                                    .FirstOrDefaultAsync(b => b.Id == id);
            });

            return result;
        }


        public async Task<BlogPost> SaveBlogPost(BlogPost blogPost, string userId)
        {
            var result = await ExecuteOnContext(async context =>
            {
                if (blogPost.Id == 0)
                {
                    //add blogpost
                    var isDuplicateTitle = await context.BlogPosts
                                            .AsNoTracking()
                                            .AnyAsync(b => b.Title == blogPost.Title);
                    if (isDuplicateTitle)
                    {
                        throw new InvalidOperationException($"Blog Post with same title '{blogPost.Title}' already exist");
                    }

                    blogPost.Slug = await GenerateSlugIfDuplicated(blogPost);
                    blogPost.CreatedAt = DateTime.UtcNow;
                    blogPost.UserId = userId;

                    if (blogPost.IsPublished)
                    {
                        blogPost.PublishedAt = DateTime.UtcNow;
                    }
                    await context.BlogPosts.AddAsync(blogPost);
                }
                else
                {
                    //edit blogpost
                    var isDuplicateTitle = await context.BlogPosts
                                            .AsNoTracking()
                                            .AnyAsync(b => b.Title == blogPost.Title && b.Id != blogPost.Id);
                    if (isDuplicateTitle)
                    {
                        throw new InvalidOperationException($"Blog Post with same title '{blogPost.Title}' already exist");
                    }

                    var dbBlogPost = await context.BlogPosts.FindAsync(blogPost.Id);

                    dbBlogPost!.Title = blogPost.Title;
                    dbBlogPost.Image = blogPost.Image;
                    dbBlogPost.Introduction = blogPost.Introduction;
                    dbBlogPost.Content = blogPost.Content;

                    dbBlogPost.CategoryId = blogPost.CategoryId;

                    dbBlogPost.IsPublished = blogPost.IsPublished;
                    dbBlogPost.IsFeatured = blogPost.IsFeatured;
                    //dbBlogPost.ViewCount = blogPost.ViewCount;
                    //dbBlogPost.PublishedAt = blogPost.PublishedAt;

                    //dbBlogPost.Slug = blogPost.Slug;
                    //dbBlogPost.CreatedAt = blogPost.CreatedAt;
                    //dbBlogPost.UserId = blogPost.UserId;

                    if (blogPost.IsPublished  && !dbBlogPost.IsPublished)
                    {
                        blogPost.PublishedAt = DateTime.UtcNow;
                    }
                    else
                    {
                        blogPost.PublishedAt = null;
                    }
                }


                await context.SaveChangesAsync();
                return blogPost;
            });

            return result;
        }


        private async Task<string> GenerateSlugIfDuplicated(BlogPost blogPost)
        {
            /*
                doblicated slug
                blog1 -> How to do this in Blazer (WASM) -> how-to do-this-in-blazor-wasm
                blog2 -> How to do this in Blazer WASM   -> how-to do-this-in-blazor-wasm
             */
            var result = await ExecuteOnContext(async context =>
            {
                string originalSlug = blogPost.Title.ToSlug();
                string slug = originalSlug;

                int count = 1;

                while (await context.BlogPosts.AsNoTracking().AnyAsync(b => b.Slug == slug))
                {
                    slug = $"{originalSlug}-{count++}";
                }
                return slug;
            });

            return result;
        }

    }
}
