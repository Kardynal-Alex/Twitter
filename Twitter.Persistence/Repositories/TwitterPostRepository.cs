using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<TwitterPost> GetTwitterPostByIdWithDetails(Guid twitterPostId)
        {
            return await context.TwitterPosts
                .Include(x => x.Images)
                .Include(x => x.User)
                .Include(x => x.Comments)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == twitterPostId);
        }

        public async Task<List<TwitterPost>> GetTwitterPostByUserIdWithDetailsExceptUserAsync(string userId)
        {
            return await context.TwitterPosts
                .Include(x => x.Images)
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<TwitterPost>> GetTwitterPostsByUserIdWithImagesAndUsers(string userId)
        {
            return await context.TwitterPosts
                .Include(x => x.Images)
                .Include(x => x.User)
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public void UpdateTwitterPost(TwitterPost twitterPost)
        {
            context.Entry(twitterPost).State = EntityState.Modified;
        }

        public async Task<List<TwitterPost>> GetFriendsTweetsByUserIdAsync(string userId)
        {
            var friendTweets = from tweet in context.TwitterPosts.Include(x => x.Images).Include(x => x.User)
                               from friend in context.Friends
                               where friend.UserId == userId && (friend.FriendId == tweet.UserId || friend.UserId == tweet.UserId)
                               select tweet;
            return await friendTweets.ToListAsync();
        }
    }
}
