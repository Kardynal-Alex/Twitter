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
    }
}
