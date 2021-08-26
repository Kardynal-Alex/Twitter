using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Contracts;
using Twitter.Domain.Entities;
using Twitter.Domain.Exceptions;
using Twitter.Domain.Repositories;
using Twitter.Services.Abstractions;

namespace Twitter.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task AddCommentAsync(CommentDTO commentDTO)
        {
            ValidateCommentDTO(commentDTO);

            commentDTO.DateCreation = DateTime.Now;
            var comment = mapper.Map<CommentDTO, Comment>(commentDTO);
            await unitOfWork.CommentRepository.AddCommentAsync(comment);

            await unitOfWork.SaveAsync();
        }

        public async Task DeleteCommentByIdAsync(Guid id)
        {
            ValidateGuidData(id);

            unitOfWork.CommentRepository.DeleteCommentById(id);

            await unitOfWork.SaveAsync();
        }

        public async Task<List<CommentDTO>> GetCommentsByTwitterPostIdAsync(Guid twitterPostId)
        {
            ValidateGuidData(twitterPostId);

            var comments = await unitOfWork.CommentRepository.GetCommentsByTwitterPostIdAsync(twitterPostId);
            return mapper.Map<List<CommentDTO>>(comments);
        }

        private void ValidateCommentDTO(CommentDTO model)
        {
            if (string.IsNullOrEmpty(model.Author) || string.IsNullOrEmpty(model.Text) ||
                string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.TwitterPostId.ToString()))
                throw new TwitterException("Incorect comment text data");
        }

        private void ValidateGuidData(Guid guid)
        {
            if (string.IsNullOrEmpty(guid.ToString()))
                throw new TwitterException("Incorect guid data");
        }

        public async Task UpdateCommentAsync(CommentDTO commentDTO)
        {
            ValidateCommentDTO(commentDTO);

            var comment = mapper.Map<CommentDTO, Comment>(commentDTO);
            unitOfWork.CommentRepository.UpdateComment(comment);
            await unitOfWork.SaveAsync();
        }
    }
}
