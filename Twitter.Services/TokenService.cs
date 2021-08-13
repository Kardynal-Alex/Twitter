using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Twitter.Domain.Entities;
using Twitter.Domain.Exceptions;
using Twitter.Services.Abstractions;
using Twitter.Services.Configurations;

namespace Twitter.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<User> userManager;
        public TokenService(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public string GenerateToken(List<Claim> claims)
        {
            var now = DateTime.UtcNow;
            var tokenOptions = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }

        public async Task<List<Claim>> GetClaims(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
                throw new TwitterException("user is not foundeed");

            var claims = new List<Claim>
            {
                new Claim("id", user.Id),
                new Claim("name", user.Name),
                new Claim("surname", user.Surname),
                new Claim("email", user.Email),
                new Claim("role", user.Role),
                new Claim("profileimagepath", user.ProfileImagePath)
            };
            return claims;
        }
    }
}
