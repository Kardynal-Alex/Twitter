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
    public class TwitterPostService : ITwitterPostService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public TwitterPostService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task AddTwitterPostsAsync(TwitterPostDTO twitterPostDTO)
        {
            ValidateTwitterPostDTO(twitterPostDTO);

            twitterPostDTO.DateCreation = DateTime.Now;
            var twitterPost = mapper.Map<TwitterPostDTO, TwitterPost>(twitterPostDTO);
            //images is added auto
            await unitOfWork.TwitterPostRepository.AddTwitterPostAsync(twitterPost);
            await unitOfWork.SaveAsync();

        }

        public async Task<List<TwitterPostDTO>> GetTwitterPostByUserIdAsync(string userId)
        {
            ValidateStringData(userId);

            var twitterPosts = await unitOfWork.TwitterPostRepository
                .GetTwitterPostByUserIdWithDetailsExceptUserAsync(userId);
            return mapper.Map<List<TwitterPostDTO>>(twitterPosts);
        }

        private void ValidateTwitterPostDTO(TwitterPostDTO model)
        {
            if (string.IsNullOrEmpty(model.PostText)) 
                throw new TwitterException("Incorect PostText length or null");

            if (!int.TryParse(model.Like.ToString(), out int like) || like < 0)
                throw new TwitterException("Incorect Like format ot less than 0");

            if (string.IsNullOrEmpty(model.UserId)) 
                throw new TwitterException("Incorect UserId length or null");
        }

        private void ValidateStringData(string model)
        {
            if (string.IsNullOrEmpty(model)) 
                throw new TwitterException("Incorect string data length or null");
        }

        public async Task DeleteTwitterPostWithImagesAsync(TwitterPostDTO twitterPostDTO)
        {
            ValidateTwitterPostDTO(twitterPostDTO);

            var twitterPost = mapper.Map<TwitterPostDTO, TwitterPost>(twitterPostDTO);
            unitOfWork.ImagesRepository.DeletePhysicalImages(twitterPost.Images);
            //delete images,comments in cascade
            unitOfWork.TwitterPostRepository.DeleteTwitterPostById(twitterPost.Id);

            await unitOfWork.SaveAsync();
        }

        public async Task<TwitterPostDTO> GetTwitterPostByIdWithDetails(Guid twitterPostId)
        {
            if (string.IsNullOrEmpty(twitterPostId.ToString()))
                throw new TwitterException("incorect guid data");

            var twitterPosts = await unitOfWork.TwitterPostRepository.GetTwitterPostByIdWithDetails(twitterPostId);
            return mapper.Map<TwitterPost, TwitterPostDTO>(twitterPosts);
        }

        public async Task<List<TwitterPostDTO>> GetTwitterPostsByUserIdWithImagesAndUsers(string userId)
        {
            ValidateStringData(userId);

            var twitterPosts = await unitOfWork.TwitterPostRepository.GetTwitterPostsByUserIdWithImagesAndUsers(userId);
            return mapper.Map<List<TwitterPostDTO>>(twitterPosts);
        }

        public async Task UpdateTwitterPostWithImagesAsync(TwitterPostDTO twitterPostDTO)
        {
            ValidateTwitterPostDTO(twitterPostDTO);

            var twitterPost = mapper.Map<TwitterPostDTO, TwitterPost>(twitterPostDTO);
            unitOfWork.TwitterPostRepository.UpdateTwitterPost(twitterPost);
            unitOfWork.ImagesRepository.UpdateImages(twitterPost.Images);

            await unitOfWork.SaveAsync();
        }

        public async Task<List<TwitterPostDTO>> GetFriendsTweetsByUserIdAsync(string userId)
        {
            ValidateStringData(userId);

            var twitterPosts = await unitOfWork.TwitterPostRepository.GetFriendsTweetsByUserIdAsync(userId);
            return mapper.Map<List<TwitterPostDTO>>(twitterPosts);
        }

        public async Task<List<TwitterPostDTO>> GetFavoriteUserTwitterPostsByUserIdAsync(string userId)
        {
            ValidateStringData(userId);

            var favoriteTweets = await unitOfWork.TwitterPostRepository.GetFavoriteUserTwitterPostsByUserIdAsync(userId);
            return mapper.Map<List<TwitterPostDTO>>(favoriteTweets);
        }

        public async Task<List<TwitterPostDTO>> SearchTwitterPostsByHeshTagAsync(string search)
        {
            var searchTeewts = await unitOfWork.TwitterPostRepository.SearchTwitterPostsByHeshTagAsync(search);
            return mapper.Map<List<TwitterPostDTO>>(searchTeewts);
        }

        public async Task UpdateOnlyTwitterPost(TwitterPostDTO twitterPostDTO)
        {
            ValidateTwitterPostDTO(twitterPostDTO);

            var twitterPost = mapper.Map<TwitterPostDTO, TwitterPost>(twitterPostDTO);
            unitOfWork.TwitterPostRepository.UpdateTwitterPost(twitterPost);

            await unitOfWork.SaveAsync();
        }
    }
}
