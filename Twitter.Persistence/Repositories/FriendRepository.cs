using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    }
}
