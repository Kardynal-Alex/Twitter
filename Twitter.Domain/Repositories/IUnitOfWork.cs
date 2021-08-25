using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using Twitter.Domain.Entities;

namespace Twitter.Domain.Repositories
{
    public interface IUnitOfWork
    {
        ITwitterPostRepository TwitterPostRepository { get; }

        IImagesRepository ImagesRepository { get; }

        ICommentRepository CommentRepository { get; }

        IUserRepository UserRepository { get; }

        IFriendRepository FriendRepository { get; }

        IFavoriteRepository FavoriteRepository { get; }

        ILikeRepository LikeRepository { get; }

        UserManager<User> UserManager { get; }
        
        SignInManager<User> SignInManager { get; }
        
        RoleManager<IdentityRole> RoleManager { get; }

        Task SaveAsync();
    }
}
