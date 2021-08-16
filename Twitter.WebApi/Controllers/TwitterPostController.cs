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
    public class TwitterPostController : ControllerBase
    {
        private readonly ITwitterPostService service;
        public TwitterPostController(ITwitterPostService service)
        {
            this.service = service;
        }

        [HttpPost("addTwitterPost")]
        public async Task<ActionResult> AddTwitterPost([FromBody] TwitterPostDTO twitterPostDTO)
        {
            await service.AddTwitterPostsAsync(twitterPostDTO);
            return Ok();
        }

        [HttpGet("getUserTwitterPosts/{id}")]
        public async Task<ActionResult<List<TwitterPostDTO>>> GetTwitterPostsByUserId(string id)
        {
            var twitterPostDTOs = await service.GetTwitterPostByUserIdAsync(id);
            return Ok(twitterPostDTOs);
        }

        [HttpPost("deleteTwitterPost")]
        public async Task<ActionResult> DeleteTwitterPost([FromBody] TwitterPostDTO twitterPostDTO)
        {
            await service.DeleteTwitterPostWithImagesAsync(twitterPostDTO);
            return Ok();
        }

        [HttpGet("getTweetByIdWithDetails/{id}")]
        public async Task<ActionResult<TwitterPostDTO>> GetTwitterPostByIdWithDetails(Guid id)
        {
            var twitterPostDTOs = await service.GetTwitterPostByIdWithDetails(id);
            return Ok(twitterPostDTOs);
        }
    }
}
