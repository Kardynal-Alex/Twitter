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
    public class LikeController : ControllerBase
    {
        private readonly ILikeService service;
        public LikeController(ILikeService service)
        {
            this.service = service;
        }

        [HttpPost("addLike")]
        public async Task<ActionResult> AddLike([FromBody] LikeDTO likeDTO)
        {
            await service.AddLikeAsync(likeDTO);
            return Ok();
        }

        [HttpDelete("deleteLikeById/{id}")]
        public async Task<ActionResult> DeleteLikeById(Guid id)
        {
            await service.DeleteLikeByIdAsync(id);
            return Ok();
        }

        [HttpGet("getLikesByUserId/{id}")]
        public async Task<ActionResult<List<LikeDTO>>> GetLikesByUserId(string id)
        {
            var likeDTOs = await service.GetLikesByUserIdAsync(id);
            return Ok(likeDTOs);
        }

        [HttpPost("getLikeByUserAndTwitterPostId")]
        public async Task<ActionResult<LikeDTO>> GetLikeByUserAndTwitterPostId([FromBody] LikeDTO likeDTO)
        {
            var expected = await service.GetLikeByUserAndTwitterPostIdAsync(likeDTO);
            return Ok(expected);
        }
    }
}
