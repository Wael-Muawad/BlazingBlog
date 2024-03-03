using BlazingBlog.Data;
using BlazingBlog.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BlazingBlog.Services.SubscribersService
{
    public class SubscriberService : ISubscriberService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public SubscriberService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory=contextFactory;
        }


        public async Task<string?> Subscribe(Subscriber subscriber)
        {
            using var context = _contextFactory.CreateDbContext();
            var isAlreadySubscribed = await context.Subscribers
                                                    .AsNoTracking()
                                                    .AnyAsync(s => s.Email == subscriber.Email);
            if (isAlreadySubscribed)
                return "You are already subscribed.";

            subscriber.SubscribedOn = DateTime.UtcNow;
            await context.Subscribers.AddAsync(subscriber);
            await context.SaveChangesAsync();
            return null;
        }
    }
}
