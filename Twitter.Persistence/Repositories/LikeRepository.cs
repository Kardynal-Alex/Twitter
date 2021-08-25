using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Domain.Entities;
using Twitter.Domain.Repositories;

namespace Twitter.Persistence.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly ApplicationContext context;
        public LikeRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task AddLikeAsync(Like like)
        {
            await context.AddAsync(like);
        }

        public void DeleteLikeById(Guid id)
        {
            context.Remove(new Like { Id = id });
        }

        public async Task<List<Like>> GetLikesByUserIdAsync(string userId)
        {
            return await context.Likes.Where(x => x.UserId == userId)
                .AsNoTracking().ToListAsync();
        }
    }
}
