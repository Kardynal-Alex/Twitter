using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Contracts;

namespace Twitter.Services.Abstractions
{
    public interface ICommentService
    {
        Task AddCommentAsync(CommentDTO commentDTO);

        Task DeleteCommentByIdAsync(Guid id);

        Task<List<CommentDTO>> GetCommentsByTwitterPostIdAsync(Guid twitterPostId);

        Task UpdateCommentAsync(CommentDTO commentDTO);
    }
}
