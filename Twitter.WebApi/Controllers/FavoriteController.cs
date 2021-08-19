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
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService service;
        public FavoriteController(IFavoriteService service)
        {
            this.service = service;
        }

        [HttpPost("addFavorite")]
        public async Task<ActionResult> AddFavorite([FromBody] FavoriteDTO favoriteDTO)
        {
            await service.AddFavoriteAsync(favoriteDTO);
            return Ok();
        }

        [HttpDelete("deleteFavoriteById/{id}")]
        public async Task<ActionResult> DeleteFavoriteById(Guid id)
        {
            await service.DeleteFavoriteByIdAsync(id);
            return Ok();
        }

        [HttpGet("getFavoritesByUserId/{id}")]
        public async Task<ActionResult<List<FavoriteDTO>>> GetFavoritesByUserId(string id)
        {
            var favoriteDTOs = await service.GetFavoritesByUserIdAsync(id);
            return Ok(favoriteDTOs);
        }
    }
}
