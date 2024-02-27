using BlazingBlog.Data.Entities;

namespace BlazingBlog.Models
{
    public record PagedResult<TResult>(TResult[] Records, int TotalCount);

    public record DetailPageModel(BlogPost BlogPost, BlogPost[] RelatedPosts)
    {
        public static DetailPageModel Embty() => new (default, []);
        public bool IsEmbty => BlogPost is null;
    }


}
