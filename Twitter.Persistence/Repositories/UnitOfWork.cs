using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Twitter.Domain.Entities;
using Twitter.Domain.Repositories;

namespace Twitter.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext context;
        public UnitOfWork(ApplicationContext context,
                          RoleManager<IdentityRole> roleManager,
                          UserManager<User> userManager,
                          SignInManager<User> signInManager)
        {
            this.context = context;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        private ITwitterPostRepository twitterPostRepository;
        public ITwitterPostRepository TwitterPostRepository
        {
            get
            {
                return twitterPostRepository ?? (twitterPostRepository = new TwitterPostRepository(context));
            }
        }

        private IImagesRepository imagesRepository;
        public IImagesRepository ImagesRepository
        {
            get
            {
                return imagesRepository ?? (imagesRepository = new ImagesRepository(context));
            }
        }

        private ICommentRepository commentRepository;
        public ICommentRepository CommentRepository
        {
            get
            {
                return commentRepository ?? (commentRepository = new CommentRepository(context));
            }
        }

        private IUserRepository userRepository;
        public IUserRepository UserRepository
        {
            get
            {
                return userRepository ?? (userRepository = new UserRepository(context));
            }
        }

        private readonly UserManager<User> userManager;
        public UserManager<User> UserManager
        {
            get
            {
                return userManager;
            }
        }
        
        private readonly SignInManager<User> signInManager;
        public SignInManager<User> SignInManager
        {
            get
            {
                return signInManager;
            }
        }
        
        private readonly RoleManager<IdentityRole> roleManager;
        public RoleManager<IdentityRole> RoleManager
        {
            get
            {
                return roleManager;
            }
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
