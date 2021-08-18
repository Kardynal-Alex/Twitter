﻿using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult> Facebook([FromBody] FacebookAuthDTO model)
        {
            var tokenString = await service.FacebookLoginAsync(model.AccessToken);
            if (tokenString != null)
            {
                return Ok(new { Token = tokenString });
            }
            return BadRequest();
        }

        [HttpPost("google")]
        public async Task<ActionResult> Google([FromBody] GoogleAuthDTO model)
        {
            var tokenString = await service.GoogleLoginAsync(model);
            if (tokenString != null)
            {
                return Ok(new { Token = tokenString });
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
    }
}
