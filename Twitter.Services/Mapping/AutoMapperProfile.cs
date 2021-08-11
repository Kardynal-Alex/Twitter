﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Twitter.Contracts;
using Twitter.Domain.Entities;

namespace Twitter.Services.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TwitterPost, TwitterPostDTO>().ReverseMap();
        }
    }
}