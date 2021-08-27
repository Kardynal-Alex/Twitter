using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Contracts;

namespace Twitter.Services.Abstractions
{
    public interface IUserService
    {
        Task<TokenAuthDTO> FacebookLoginAsync(string accessToken);

        Task<TokenAuthDTO> GoogleLoginAsync(GoogleAuthDTO googleAuthDTO);

        Task<UserDTO> GetUserByUserIdAsync(string userId);

        Task<List<UserDTO>> SearchUsersByNameAndSurnameAsync(string search);

        Task<List<UserDTO>> GetUserFriendsByUserIdAsync(string userId);

        Task<TokenAuthDTO> RefreshTokenAsync(TokenAuthDTO tokenAuthDTO);

        Task UpdateUserProfileAsync(UserDTO userDTO);
    }
}
