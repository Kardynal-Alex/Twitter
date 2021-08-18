using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Twitter.Domain.Entities;
using Twitter.Domain.Repositories;
using Twitter.Persistence.Repositories;

namespace Twitter.Persistence.Configurations
{
    public static class PersistenceDependencies
    {
        public static IServiceCollection AddPersistenceDependencies(this IServiceCollection services, IConfiguration Configuration)
        {
            string conString = Configuration["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<ApplicationContext>(option => option.UseSqlServer(conString));

            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;

            }).AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ITwitterPostRepository, TwitterPostRepository>();
            services.AddTransient<IImagesRepository, ImagesRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IFriendRepository, FriendRepository>();

            return services;
        }
    }
}
