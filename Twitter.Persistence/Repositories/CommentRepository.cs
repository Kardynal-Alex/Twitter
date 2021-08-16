using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Domain.Entities;
using Twitter.Domain.Repositories;

namespace Twitter.Persistence.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationContext context;
        public CommentRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task AddCommentAsync(Comment comment)
        {
            await context.Comments.AddAsync(comment);
        }

        public void DeleteCommentById(Guid id)
        {
            context.Comments.Remove(new Comment { Id = id });
        }

        public async Task<List<Comment>> GetCommentsByTwitterPostIdAsync(Guid twitterPostId)
        {
            return await context.Comments
                .Where(x => x.TwitterPostId == twitterPostId)
                .AsNoTracking()
                .ToListAsync();
        }

        public void UpdateComment(Comment comment)
        {
            context.Comments.Update(comment);
        }
    }
}
