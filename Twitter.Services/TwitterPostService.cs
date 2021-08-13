using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
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

        private void ValidateTwitterPostDTO(TwitterPostDTO model)
        {
            if (model.PostText == null || model.PostText.Length == 0)
                throw new TwitterException("Incorect PostText length or null");

            if (!int.TryParse(model.Like.ToString(), out int like) || like < 0)
                throw new TwitterException("Incorect Like format ot less than 0");

            if (model.UserId == null || model.UserId.Length == 0)
                throw new TwitterException("Incorect UserId length or null");
        }
    }
}
