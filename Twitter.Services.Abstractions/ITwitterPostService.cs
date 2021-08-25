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

        Task<List<TwitterPostDTO>> GetFriendsTweetsByUserIdAsync(string userId);

        Task<List<TwitterPostDTO>> GetFavoriteUserTwitterPostsByUserIdAsync(string userId);

        Task<List<TwitterPostDTO>> SearchTwitterPostsByHeshTagAsync(string search);

        Task UpdateOnlyTwitterPost(TwitterPostDTO twitterPostDTO);
    }
}
