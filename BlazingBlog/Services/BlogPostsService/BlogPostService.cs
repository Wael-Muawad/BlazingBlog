using BlazingBlog.Data;
using BlazingBlog.Data.Entities;
using BlazingBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazingBlog.Services.BlogPostsService
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public BlogPostService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        //using the template pattern
        private async Task<TResult> ExecuteOnContext<TResult>(Func<ApplicationDbContext, Task<TResult>> contextFactory)
        {
            using var context = _contextFactory.CreateDbContext();
            return await contextFactory.Invoke(context);
        }


        public async Task<BlogPost[]> GetFeaturedBlogPosts(int count, int categoryId = 0)
        {
            var result = await ExecuteOnContext(async context =>
            {
                var query = context.BlogPosts
                                        .AsNoTracking()
                                        .Include(b => b.Category)
                                        .Include(b => b.User)
                                        .Where(b => b.IsPublished);

                if (categoryId > 0)
                {
                    query = query.Where(b => b.CategoryId == categoryId);
                }

                var records = await query.Where(b => b.IsFeatured)
                                         .OrderBy(b => Guid.NewGuid())
                                         .Take(count)
                                         .ToArrayAsync();

                if (count < records.Length)
                {
                    var additionalRecords = await query.Where(b => b.IsFeatured)
                                                       .OrderBy(b => Guid.NewGuid())
                                                       .Take(count - records.Length)
                                                       .ToArrayAsync();
                    records = [.. records, .. additionalRecords];
                }

                return records;
            });

            return result;
        }
        public async Task<BlogPost[]> GetPopularBlogPosts(int count, int categoryId = 0)
        {
            var result = await ExecuteOnContext(async context =>
            {
                var query = context.BlogPosts
                                        .AsNoTracking()
                                        .Include(b => b.Category)
                                        .Include(b => b.User)
                                        .Where(b => b.IsPublished);

                if (categoryId > 0)
                {
                    query = query.Where(b => b.CategoryId == categoryId);
                }

                return await query.OrderByDescending(B => B.ViewCount)
                            .Take(count)
                            .ToArrayAsync();
            });

            return result;
        }
        public async Task<BlogPost[]> GetRecentBlogPosts(int count, int categoryId = 0)
        {
            var result = await ExecuteOnContext(async context =>
            {
                var query = context.BlogPosts
                                        .AsNoTracking()
                                        .Include(b => b.Category)
                                        .Include(b => b.User)
                                        .Where(b => b.IsPublished);

                if (categoryId > 0)
                {
                    query = query.Where(b => b.CategoryId == categoryId);
                }

                return await query.OrderByDescending(b => b.PublishedAt)
                            .Take(count)
                            .ToArrayAsync();
            });

            return result;
        }



        public async Task<DetailPageModel> GetBlogPostBySlog(string slug)
        {
            var result = await ExecuteOnContext(async context =>
            {
                var blogPost = context.BlogPosts
                                        .AsNoTracking()
                                        .Include(b => b.Category)
                                        .Include(b => b.User)
                                        .FirstOrDefault(b => b.Slug == slug && b.IsPublished);

                if (blogPost is null)
                    return DetailPageModel.Embty();

                var relatedPosts = await context.BlogPosts
                                            .AsNoTracking()
                                            .Include(b => b.Category)
                                            .Include(b => b.User)
                                            .Where(b => b.CategoryId == blogPost.CategoryId && b.IsPublished)
                                            .OrderBy(b => Guid.NewGuid())
                                            .Take(4)
                                            .ToArrayAsync();

                return new DetailPageModel(blogPost, relatedPosts);
            });

            return result;
        }
    }
}
