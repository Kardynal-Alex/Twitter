using AutoMapper;
using System;
using Twitter.Contracts;
using Twitter.Domain.Entities;

namespace Twitter.Services.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TwitterPost, TwitterPostDTO>().ReverseMap();
            CreateMap<Images, ImagesDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Comment, CommentDTO>().ReverseMap();
            CreateMap<Friend, FriendDTO>().ReverseMap();
            CreateMap<Favorite, FavoriteDTO>().ReverseMap();
        }
    }
}
