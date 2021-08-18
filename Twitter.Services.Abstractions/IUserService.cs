using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Contracts;

namespace Twitter.Services.Abstractions
{
    public interface IUserService
    {
        Task<string> FacebookLoginAsync(string accessToken);

        Task<string> GoogleLoginAsync(GoogleAuthDTO googleAuthDTO);

        Task<UserDTO> GetUserByUserIdAsync(string userId);

        Task<List<UserDTO>> SearchUsersByNameAndSurnameAsync(string search);
    }
}
