using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Domain.Entities;

namespace Twitter.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> SearchUserByNameAndSurname(string search);

        Task<List<User>> GetUserFriendsByUserIdAsync(string userId);

        Task<List<User>> GetUserFollowersAsync(string userFriendId);
    }
}
