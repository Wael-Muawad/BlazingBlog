using BlazingBlog.Data;
using BlazingBlog.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazingBlog.Services.BlogPostsService
{
    public class BlogPostService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public BlogPostService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<BlogPost[]> GetFeaturedBlogPosts(int count, int categoryId = 0) { }
        public async Task<BlogPost[]> GetPopularBlogPosts(int count, int categoryId = 0) { }
        public async Task<BlogPost[]> GetRecentBlogPosts(int count, int categoryId = 0) { }


    }
}
