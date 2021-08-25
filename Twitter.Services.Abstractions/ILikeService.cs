using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Contracts;

namespace Twitter.Services.Abstractions
{
    public interface ILikeService
    {
        Task AddLikeAsync(LikeDTO likeDTO);

        Task DeleteLikeByIdAsync(Guid id);

        Task<List<LikeDTO>> GetLikesByUserIdAsync(string userId);

        Task<LikeDTO> GetLikeByUserAndTwitterPostIdAsync(LikeDTO likeDTO);
    }
}
