using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Domain.Entities;
using Twitter.Domain.Repositories;

namespace Twitter.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext context;
        public UserRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<List<User>> SearchUserByNameAndSurname(string search)
        {
            var users = await context.Users.FromSqlRaw($"SELECT * FROM dbo.AspNetUsers WHERE CONCAT(Name,' ',Surname) LIKE '%{search}%'")
                .ToListAsync();
            return users;
        }

        public async Task<List<User>> GetUserFriendsByUserIdAsync(string userId)
        {
            var query = $@" SELECT u.Id,u.Name,u.Surname,u.Role,u.ProfileImagePath,u.UserName,u.NormalizedUserName,u.Email,u.NormalizedEmail,
                            u.EmailConfirmed,u.PasswordHash,u.SecurityStamp,u.ConcurrencyStamp,u.PhoneNumber,u.PhoneNumberConfirmed,
                            u.TwoFactorEnabled,u.LockoutEnd,u.LockoutEnabled,u.AccessFailedCount
                            FROM dbo.AspNetUsers as u
                            INNER JOIN dbo.Friends as f
                            ON f.UserId='{userId}' AND u.Id=f.FriendId";
            return await context.Users.FromSqlRaw(query).ToListAsync();
        }
    }
}
