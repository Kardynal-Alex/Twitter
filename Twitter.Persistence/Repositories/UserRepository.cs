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
    }
}
