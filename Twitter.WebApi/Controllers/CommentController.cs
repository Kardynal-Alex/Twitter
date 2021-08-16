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
    public class CommentController : ControllerBase
    {
        private readonly ICommentService service;
        public CommentController(ICommentService service)
        {
            this.service = service;
        }

        [HttpGet("getCommentsByTweetId/{id}")]
        public async Task<ActionResult<List<CommentDTO>>> GetCommentsByTwitterPostId(Guid id)
        {
            var commentDTOs = await service.GetCommentsByTwitterPostIdAsync(id);
            return Ok(commentDTOs);
        }

        [HttpPost("addComment")]
        public async Task<ActionResult> AddComment([FromBody] CommentDTO commentDTO)
        {
            await service.AddCommentAsync(commentDTO);
            return Ok();
        }

        [HttpDelete("deleteCommentById/{id}")]
        public async Task<ActionResult> DeleteCommentById(Guid id)
        {
            await service.DeleteCommentByIdAsync(id);
            return Ok();
        }
    }
}
