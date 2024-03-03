using BlazingBlog.Data.Entities;
using BlazingBlog.Models;

namespace BlazingBlog.Services.SubscribersService
{
    public interface ISubscriberService
    {
        Task<string?> Subscribe(Subscriber subscriber);
        Task<PagedResult<Subscriber>> GetSubscribers(int startIndex, int pageSize);

    }
}