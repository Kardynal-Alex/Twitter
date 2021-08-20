using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Domain.Entities;

namespace Twitter.Domain.Repositories
{
    public interface IFavoriteRepository
    {
        Task AddFavoriteAsync(Favorite favorite);

        void DeleteFavoriteById(Guid id);

        Task<List<Favorite>> GetFavoritesByUserIdAsync(string userId);

        Task DeleteFavoriteByTwitterPostAndUserIdAsync(Favorite favorite);
    }
}
