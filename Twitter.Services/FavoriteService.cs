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
    public class FavoriteService : IFavoriteService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public FavoriteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task AddFavoriteAsync(FavoriteDTO favoriteDTO)
        {
            ValidateFavoriteDTOData(favoriteDTO);

            var favorite = mapper.Map<FavoriteDTO, Favorite>(favoriteDTO);
            await unitOfWork.FavoriteRepository.AddFavoriteAsync(favorite);

            await unitOfWork.SaveAsync();
        }

        private void ValidateFavoriteDTOData(FavoriteDTO model)
        {
            if (string.IsNullOrEmpty(model.UserId) || !Guid.TryParse(model.TwitterPostId.ToString(), out Guid _))
                throw new TwitterException("Incorect favoriteDTO data");
        }

        public async Task DeleteFavoriteByIdAsync(Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
                throw new TwitterException("incorect guid data");

            unitOfWork.FavoriteRepository.DeleteFavoriteById(id);

            await unitOfWork.SaveAsync();
        }

        public async Task<List<FavoriteDTO>> GetFavoritesByUserIdAsync(string userId)
        {
            ValidateStringData(userId);

            var favorites = await unitOfWork.FavoriteRepository.GetFavoritesByUserIdAsync(userId);
            return mapper.Map<List<FavoriteDTO>>(favorites);
        }

        private void ValidateStringData(string model)
        {
            if (string.IsNullOrEmpty(model))
                throw new TwitterException("Incorect string data length or null");
        }

        public async Task DeleteFavoriteByTwitterPostAndUserIdAsync(FavoriteDTO favoriteDTO)
        {
            ValidateFavoriteDTOData(favoriteDTO);

            var favorite = mapper.Map<FavoriteDTO, Favorite>(favoriteDTO);
            await unitOfWork.FavoriteRepository.DeleteFavoriteByTwitterPostAndUserIdAsync(favorite);

            await unitOfWork.SaveAsync();
        }
    }
}
