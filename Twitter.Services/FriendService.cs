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
    public class FriendService : IFriendService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public FriendService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task AddFriendAsync(FriendDTO friendDTO)
        {
            ValidateFriendDTO(friendDTO);

            var friend = mapper.Map<FriendDTO, Friend>(friendDTO);
            await unitOfWork.FriendRepository.AddFriendAsync(friend);

            await unitOfWork.SaveAsync();
        }

        private void ValidateFriendDTO(FriendDTO model)
        {
            if (string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.FriendId)) 
                throw new TwitterException("Incorext friend DTO data");
        }

        public async Task DeleteFriendByIdAsync(Guid id)
        {
            ValidateGuidData(id);

            unitOfWork.FriendRepository.DeleteFriendById(id);

            await unitOfWork.SaveAsync();
        }

        public async Task<FriendDTO> GetFriendByIdAsync(Guid id)
        {
            ValidateGuidData(id);

            var friend = await unitOfWork.FriendRepository.GetFriendByIdAsync(id);
            return mapper.Map<Friend, FriendDTO>(friend);
        }

        private void ValidateGuidData(Guid id)
        {
            if (!Guid.TryParse(id.ToString(), out Guid guid) || string.IsNullOrEmpty(guid.ToString()))
                throw new TwitterException("Incorect Guid data");
        }
    }
}
