using BlazingBlog.Data;
using BlazingBlog.Data.Entities;
using BlazingBlog.Models;
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


        public async Task<PagedResult<Subscriber>> GetSubscribers(int startIndex, int pageSize)
        {
            using var context = _contextFactory.CreateDbContext();

            var query = context.Subscribers
                                 .AsNoTracking()
                                 .OrderByDescending(s => s.SubscribedOn);

            var count = await query.CountAsync();


            var results = await query
                                 .Skip(startIndex)
                                 .Take(pageSize)
                                 .ToArrayAsync();


            return new PagedResult<Subscriber>(results, count);
        }

    }
}
