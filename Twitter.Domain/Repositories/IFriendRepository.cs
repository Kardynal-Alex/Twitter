using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Domain.Entities;

namespace Twitter.Domain.Repositories
{
    public interface IFriendRepository
    {
        Task AddFriendAsync(Friend friend);

        void DeleteFriendById(Guid id);

        Task<Friend> GetFriendByIdAsync(Guid id);
    }
}
