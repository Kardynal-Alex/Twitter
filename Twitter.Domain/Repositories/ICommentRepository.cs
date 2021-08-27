using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Twitter.Domain.Entities;

namespace Twitter.Domain.Repositories
{
    public interface ICommentRepository
    {
        Task AddCommentAsync(Comment comment);

        void UpdateComment(Comment comment);

        void DeleteCommentById(Guid id);

        Task<List<Comment>> GetCommentsByTwitterPostIdAsync(Guid twitterPostId);

        void UpdateComments(List<Comment> comments);

        Task<List<Comment>> GetUserCommentsByUserIdAsync(string userId);
    }
}
