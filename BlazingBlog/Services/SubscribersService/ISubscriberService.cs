using BlazingBlog.Data.Entities;

namespace BlazingBlog.Services.SubscribersService
{
    public interface ISubscriberService
    {
        Task<string?> Subscribe(Subscriber subscriber);
    }
}