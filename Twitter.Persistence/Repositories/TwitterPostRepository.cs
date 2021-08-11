using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Domain.Entities;
using Twitter.Domain.Repositories;

namespace Twitter.Persistence.Repositories
{
    public class TwitterPostRepository : ITwitterPostRepository
    {
        private readonly ApplicationContext context;
        public TwitterPostRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task AddTwitterPostAsync(TwitterPost twitterPost)
        {
            await context.TwitterPosts.AddAsync(twitterPost);
        }

        public void DeleteTwitterPostById(Guid id)
        {
            context.TwitterPosts.Remove(new TwitterPost { Id = id });
        }

        public void UpdateTwitterPost(TwitterPost twitterPost)
        {
            context.Entry(twitterPost).State = EntityState.Modified;
        }
    }
}
