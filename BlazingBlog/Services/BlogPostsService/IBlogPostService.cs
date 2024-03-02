using BlazingBlog.Data.Entities;
using BlazingBlog.Models;

namespace BlazingBlog.Services.BlogPostsService
{
    public interface IBlogPostService
    {
        Task<DetailPageModel> GetBlogPostBySlog(string slug);
        Task<BlogPost[]> GetFeaturedBlogPosts(int count, int categoryId = 0);
        Task<BlogPost[]> GetPopularBlogPosts(int count, int categoryId = 0);
        Task<BlogPost[]> GetRecentBlogPosts(int count, int categoryId = 0);
        Task<BlogPost[]> GetAllCategoryBlogPosts(int pageIndex, int pageSize, int categoryId = 0);
    }
}