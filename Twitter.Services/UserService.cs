using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Twitter.Contracts;
using Twitter.Domain.Entities;
using Twitter.Domain.Exceptions;
using Twitter.Domain.Repositories;
using Twitter.Services.Abstractions;

namespace Twitter.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITokenService tokenService;
        public UserService(IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            this.unitOfWork = unitOfWork;
            this.tokenService = tokenService;
        }

        public async Task<string> FacebookLoginAsync(string accessToken)
        {
            HttpClient Client = new HttpClient();
            // 1.generate an app access token
            var appAccessTokenResponse = await Client.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={fbAuthSettings.AppId}&client_secret={fbAuthSettings.AppSecret}&grant_type=client_credentials");
            var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);
            // 2. validate the user access token
            var userAccessTokenValidationResponse = await Client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={accessToken}&access_token={appAccessToken.AccessToken}");
            var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

            if (!userAccessTokenValidation.Data.IsValid)
            {
                throw new TwitterException("Invalid facebook token");
            }
            // 3. we've got a valid token so we can request user data from fb
            var userInfoResponse = await Client.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name&access_token={accessToken}");
            var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

            var user = await unitOfWork.UserManager.FindByEmailAsync(userInfo.Email);
            if (user == null)
            {
                var newUser = new User
                {
                    Email = userInfo.Email,
                    Name = userInfo.FirstName,
                    Surname = userInfo.LastName,
                    UserName = userInfo.Email,
                    Role = "user",
                    EmailConfirmed = true,
                    ProfileImagePath = userInfo.Picture.Data.Url
                };
                var result = await unitOfWork.UserManager.CreateAsync(newUser, Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8));
                if (!result.Succeeded)
                    throw new TwitterException("Cannot create user");
                await unitOfWork.UserManager.AddToRoleAsync(newUser, newUser.Role);
            }

            AuthenticationProperties properties = new AuthenticationProperties
            {
                IsPersistent = true
            };
            await unitOfWork.SignInManager.SignInAsync(user, properties);

            var claims = await tokenService.GetClaims(userInfo.Email);
            var token = tokenService.GenerateToken(claims);
            return token;
        }

        public async Task<string> GoogleLoginAsync(GoogleAuthDTO googleAuthDTO)
        {
            throw new NotImplementedException();
        }
    }
}
