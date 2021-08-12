﻿using System.Threading.Tasks;
using Twitter.Contracts;

namespace Twitter.Services.Abstractions
{
    public interface IUserService
    {
        Task<string> FacebookLoginAsync(string accessToken);
        Task<string> GoogleLoginAsync(GoogleAuthDTO googleAuthDTO);
    }
}
