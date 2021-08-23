using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Twitter.Contracts;
using Twitter.Domain.Entities;
using Twitter.Domain.Exceptions;
using Twitter.Domain.Repositories;
using Twitter.Services.Abstractions;
using Twitter.Services.Configurations;

namespace Twitter.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;
        private readonly FacebookAuthSettings fbAuthSettings;
        private readonly GoogleAuthSettings googleAuthSettings;
        public UserService(IUnitOfWork unitOfWork,
                           ITokenService tokenService,
                           IMapper mapper,
                           IOptions<FacebookAuthSettings> fbAuthSettingsAccessor,
                           IOptions<GoogleAuthSettings> googlelAuthSettingsAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.tokenService = tokenService;
            this.mapper = mapper;
            fbAuthSettings = fbAuthSettingsAccessor.Value;
            googleAuthSettings = googlelAuthSettingsAccessor.Value;
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
            var userInfoResponse = await Client.GetStringAsync($"https://graph.facebook.com/v11.0/me?fields=id,email,first_name,last_name,picture&access_token={accessToken}&debug=all");
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

            var claims = await tokenService.GetClaims(userInfo.Email);
            var token = tokenService.GenerateToken(claims);
            return token;
        }

        public async Task<UserDTO> GetUserByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new TwitterException("UserId is null or empty");

            var user = await unitOfWork.UserManager.FindByIdAsync(userId);
            return mapper.Map<User, UserDTO>(user);
        }

        public async Task<TokenAuthDTO> GoogleLoginAsync(GoogleAuthDTO googleAuthDTO)
        {
            var payload = await VerifyGoogleToken(googleAuthDTO);
            if (payload == null)
                throw new TwitterException("Invalid Google Authentication");

            var info = new UserLoginInfo(googleAuthDTO.Provider, payload.Subject, googleAuthDTO.Provider);
            var user = await unitOfWork.UserManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            if (user == null)
            {
                user = await unitOfWork.UserManager.FindByEmailAsync(payload.Email);

                if (user == null)
                {
                    user = new User
                    {
                        Email = payload.Email,
                        UserName = payload.Email,
                        Role = "user",
                        Name = payload.FamilyName,
                        Surname = payload.GivenName,
                        EmailConfirmed = payload.EmailVerified,
                        ProfileImagePath = payload.Picture
                    };
                    var result = await unitOfWork.UserManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        await unitOfWork.UserManager.AddToRoleAsync(user, user.Role);
                        await unitOfWork.UserManager.AddLoginAsync(user, info);
                    }
                    else
                    {
                        throw new TwitterException("Invalid External Authentication");
                    }
                }
                else
                {
                    await unitOfWork.UserManager.AddLoginAsync(user, info);
                }
            }
            if (user == null)
                throw new TwitterException("Invalid External Authentication");

            var claims = await tokenService.GetClaims(user.Email);
            var token = tokenService.GenerateToken(claims);
            var refreshToken = tokenService.GenerateRefreshToken();
            return new TokenAuthDTO
            {
                Token = token,
                RefreshToken = refreshToken
            };
        }

        private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(GoogleAuthDTO externalAuth)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { googleAuthSettings.ClientId }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.IdToken, settings);
                return payload;
            }
            catch (Exception ex)
            {
                throw new TwitterException(ex.Message);
            }
        }

        public async Task<List<UserDTO>> SearchUsersByNameAndSurnameAsync(string search)
        {
            var users = await unitOfWork.UserRepository.SearchUserByNameAndSurname(search);
            return mapper.Map<List<UserDTO>>(users);
        }

        public async Task<List<UserDTO>> GetUserFriendsByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new TwitterException("Incorect userId");

            var userFriends = await unitOfWork.UserRepository.GetUserFriendsByUserIdAsync(userId);
            return mapper.Map<List<UserDTO>>(userFriends);
        }
    }
}
