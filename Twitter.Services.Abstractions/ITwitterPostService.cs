using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Contracts;

namespace Twitter.Services.Abstractions
{
    public interface ITwitterPostService
    {
        Task AddTwitterPostsAsync(TwitterPostDTO twitterPostDTO);

        Task<List<TwitterPostDTO>> GetTwitterPostByUserIdAsync(string userId);

        Task UpdateTwitterPostWithImagesAsync(TwitterPostDTO twitterPostDTO);

        Task DeleteTwitterPostWithImagesAsync(TwitterPostDTO twitterPostDTO);

        Task<TwitterPostDTO> GetTwitterPostByIdWithDetails(Guid twitterPostId);

        Task<List<TwitterPostDTO>> GetTwitterPostsByUserIdWithImagesAndUsers(string userId);
    }
}
