using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Contracts;
using Twitter.Services.Abstractions;

namespace Twitter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService service;
        public AccountController(IUserService service)
        {
            this.service = service;
        }

        [HttpPost("facebook")]
        public async Task<ActionResult<TokenAuthDTO>> Facebook([FromBody] FacebookAuthDTO model)
        {
            var tokenAuthDTO = await service.FacebookLoginAsync(model.AccessToken);
            if (tokenAuthDTO != null)
            {
                return Ok(tokenAuthDTO);
            }
            return BadRequest();
        }

        [HttpPost("google")]
        public async Task<ActionResult<TokenAuthDTO>> Google([FromBody] GoogleAuthDTO model)
        {
            var tokenAuthDTO = await service.GoogleLoginAsync(model);
            if (tokenAuthDTO != null)
            {
                return Ok(tokenAuthDTO);
            }
            return BadRequest();
        }

        [HttpGet("getUserById/{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(string id)
        {
            var userDTO = await service.GetUserByUserIdAsync(id);
            return Ok(userDTO);
        }

        [HttpGet("SearchUsers/")]
        public async Task<ActionResult<List<UserDTO>>> SearchUsersByNameAndSurname(string search)
        {
            var userDTOs = await service.SearchUsersByNameAndSurnameAsync(search);
            return Ok(userDTOs);
        }

        [HttpGet("getUserFriendsByUserId/{id}")]
        public async Task<ActionResult<List<UserDTO>>> GetUserFriendsByUserId(string id)
        {
            var userFriendDTOs = await service.GetUserFriendsByUserIdAsync(id);
            return Ok(userFriendDTOs);
        }

        [HttpPost("refreshToken")]
        public async Task<ActionResult<TokenAuthDTO>> RefreshToken(TokenAuthDTO tokenAuthDTO)
        {
            var result = await service.RefreshTokenAsync(tokenAuthDTO);
            if(result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        
        [HttpPut("updateUser")]
        public async Task<ActionResult> UpdateUserProfile([FromBody] UserDTO userDTO)
        {
            await service.UpdateUserProfileAsync(userDTO);
            return Ok();
        }

        [HttpGet("getUserFollowers/{id}")]
        public async Task<ActionResult<List<UserDTO>>> GetUserFollowers(string id)
        {
            var followers = await service.GetUserFollowersAsync(id);
            return Ok(followers);
        }
    }
}
