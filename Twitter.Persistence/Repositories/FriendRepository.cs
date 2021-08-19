using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Domain.Entities;
using Twitter.Domain.Repositories;

namespace Twitter.Persistence.Repositories
{
    public class FriendRepository : IFriendRepository
    {
        private readonly ApplicationContext context;
        public FriendRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task AddFriendAsync(Friend friend)
        {
            await context.Friends.AddAsync(friend);
        }

        public void DeleteFriendById(Guid id)
        {
            context.Friends.Remove(new Friend { Id = id });
        }

        public async Task<Friend> GetFriendByIdAsync(Guid id)
        {
            return await context.Friends.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Friend> GetFriendByUserAndFriendIdAsync(Friend friend)
        {
            return await context.Friends.FirstOrDefaultAsync(x => x.UserId == friend.UserId && x.FriendId == friend.FriendId);
        }

        public async Task<List<Friend>> GetFriendsByUserIdAsync(string userId)
        {
            return await context.Friends.Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
