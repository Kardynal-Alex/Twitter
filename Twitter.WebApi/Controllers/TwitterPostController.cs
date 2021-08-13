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
    }
}
