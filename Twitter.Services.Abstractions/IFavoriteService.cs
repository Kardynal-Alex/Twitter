using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Contracts;

namespace Twitter.Services.Abstractions
{
    public interface IFavoriteService
    {
        Task AddFavoriteAsync(FavoriteDTO favoriteDTO);

        Task DeleteFavoriteByIdAsync(Guid id);

        Task<List<FavoriteDTO>> GetFavoritesByUserIdAsync(string userId);

        Task DeleteFavoriteByTwitterPostAndUserIdAsync(FavoriteDTO favoriteDTO);
    }
}
