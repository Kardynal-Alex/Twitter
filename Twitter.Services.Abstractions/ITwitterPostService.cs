using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Twitter.Contracts;

namespace Twitter.Services.Abstractions
{
    public interface ITwitterPostService
    {
        Task AddTwitterPostsAsync(TwitterPostDTO twitterPostDTO);

        Task<List<TwitterPostDTO>> GetTwitterPostByUserIdAsync(string userId);
    }
}
