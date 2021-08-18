using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Contracts;

namespace Twitter.Services.Abstractions
{
    public interface IFriendService
    {
        Task AddFriendAsync(FriendDTO friend);

        Task DeleteFriendByIdAsync(Guid id);

        Task<FriendDTO> GetFriendByIdAsync(Guid id);
    }
}
