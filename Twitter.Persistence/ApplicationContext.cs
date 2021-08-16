using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Twitter.Domain.Entities;

namespace Twitter.Persistence
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> opts) : base(opts) { }

        public DbSet<TwitterPost> TwitterPosts { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<TwitterPost>()
                        .HasOne(x => x.User)
                        .WithMany(x => x.TwitterPosts)
                        .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<TwitterPost>()
                        .HasOne(x => x.Images)
                        .WithOne(x => x.TwitterPost)
                        .HasForeignKey<Images>(x => x.Id)
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<TwitterPost>()
                        .HasMany(x => x.Comments)
                        .WithOne(x => x.TwitterPost)
                        .HasForeignKey(x => x.TwitterPostId)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
