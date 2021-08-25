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
    public class LikeService : ILikeService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public LikeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task AddLikeAsync(LikeDTO likeDTO)
        {
            ValidateLikeDTO(likeDTO);

            var like = mapper.Map<LikeDTO, Like>(likeDTO);
            await unitOfWork.LikeRepository.AddLikeAsync(like);

            await unitOfWork.SaveAsync();
        }

        private void ValidateLikeDTO(LikeDTO model)
        {
            if (string.IsNullOrEmpty(model.UserId) || !Guid.TryParse(model.TwitterPostId.ToString(), out Guid _))
                throw new TwitterException("Incorect favoriteDTO data");
        }

        public async Task DeleteLikeByIdAsync(Guid id)
        {
            if (!Guid.TryParse(id.ToString(), out Guid _))
                throw new TwitterException("Incorect Guid data");

            unitOfWork.LikeRepository.DeleteLikeById(id);
            await unitOfWork.SaveAsync();
        }

        public async Task<List<LikeDTO>> GetLikesByUserIdAsync(string userId)
        {
            ValidateStringData(userId);

            var likes = await unitOfWork.LikeRepository.GetLikesByUserIdAsync(userId);
            return mapper.Map<List<LikeDTO>>(likes);
        }

        private void ValidateStringData(string model)
        {
            if (string.IsNullOrEmpty(model))
                throw new TwitterException("Incorect string data length or null");
        }
    }
}
