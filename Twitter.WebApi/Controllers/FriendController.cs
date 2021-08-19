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
    public class FriendController : ControllerBase
    {
        private readonly IFriendService service;
        public FriendController(IFriendService service)
        {
            this.service = service;
        }

        [HttpPost("addFriend")]
        public async Task<ActionResult> AddFriend([FromBody] FriendDTO friendDTO)
        {
            await service.AddFriendAsync(friendDTO);
            return Ok();
        }

        [HttpDelete("deleteFriendById/{id}")]
        public async Task<ActionResult> DeleteFriendById(Guid id)
        {
            await service.DeleteFriendByIdAsync(id);
            return Ok();
        }

        [HttpGet("getFriendById/{id}")]
        public async Task<ActionResult<FriendDTO>> GetFriendById(Guid id)
        {
            var friendDTO = await service.GetFriendByIdAsync(id);
            return Ok(friendDTO);
        }

        [HttpPost("getFriendByUserAndFriendId")]
        public async Task<ActionResult<FriendDTO>> GetFriendByUserAndFriendId([FromBody] FriendDTO friendDTO)
        {
            var result = await service.GetFriendByUserAndFriendIdAsync(friendDTO);
            return Ok(result);
        }

        [HttpGet("getFriendsByUserId/{id}")]
        public async Task<ActionResult<List<FriendDTO>>> GetFriendsByUserId(string id)
        {
            var friendDTOs = await service.GetFriendsByUserIdAsync(id);
            return Ok(friendDTOs);
        }
    }
}
