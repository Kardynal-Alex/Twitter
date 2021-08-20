using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Domain.Entities;
using Twitter.Domain.Repositories;

namespace Twitter.Persistence.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly ApplicationContext context;
        public FavoriteRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task AddFavoriteAsync(Favorite favorite)
        {
            await context.Favorites.AddAsync(favorite);
        }

        public void DeleteFavoriteById(Guid id)
        {
            context.Favorites.Remove(new Favorite { Id = id });
        }

        public async Task<List<Favorite>> GetFavoritesByUserIdAsync(string userId)
        {
            return await context.Favorites
                .Where(x => x.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task DeleteFavoriteByTwitterPostAndUserIdAsync(Favorite favorite)
        {
            var favoriteToDelete = await context.Favorites
                .FirstOrDefaultAsync(x => x.TwitterPostId == favorite.TwitterPostId && x.UserId == favorite.UserId);
            context.Favorites.Remove(favoriteToDelete);
        }
    }
}
