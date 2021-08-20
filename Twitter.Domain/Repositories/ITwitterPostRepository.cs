using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Domain.Entities;

namespace Twitter.Domain.Repositories
{
    public interface ITwitterPostRepository
    {
        Task AddTwitterPostAsync(TwitterPost twitterPost);
        
        void UpdateTwitterPost(TwitterPost twitterPost);

        void DeleteTwitterPostById(Guid id);

        Task<List<TwitterPost>> GetTwitterPostByUserIdWithDetailsExceptUserAsync(string userId);

        Task<TwitterPost> GetTwitterPostByIdWithDetails(Guid twitterPostId);

        Task<List<TwitterPost>> GetTwitterPostsByUserIdWithImagesAndUsers(string userId);

        Task<List<TwitterPost>> GetFriendsTweetsByUserIdAsync(string userId);

        Task<List<TwitterPost>> GetFavoriteUserTwitterPostsByUserIdAsync(string userId);
    }
}
