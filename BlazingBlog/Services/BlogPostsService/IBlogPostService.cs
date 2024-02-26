using BlazingBlog.Data.Entities;
using BlazingBlog.Models;

namespace BlazingBlog.Services.BlogPostsService
{
    public interface IBlogPostService
    {
        Task<BlogPost?> GetBlogPostById(int id);
        Task<PagedResult<BlogPost>> GetBlogPosts(int startIndex, int pageSize);
        Task<BlogPost> SaveBlogPost(BlogPost blogPost, string userId);
    }
}