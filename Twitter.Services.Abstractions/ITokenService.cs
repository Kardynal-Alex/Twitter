using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Services.Abstractions
{
    public interface ITokenService
    {
        Task<List<Claim>> GetClaims(string email);

        string GenerateToken(List<Claim> claims);

        string GenerateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
