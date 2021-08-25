using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Domain.Entities;

namespace Twitter.Domain.Repositories
{
    public interface ILikeRepository
    {
        Task AddLikeAsync(Like like);

        void DeleteLikeById(Guid id);

        Task<List<Like>> GetLikesByUserIdAsync(string userId);

        Task<Like> GetLikeByUserAndTwitterPostIdAsync(Like like);
    }
}
