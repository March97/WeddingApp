using System.Threading.Tasks;
using WeddingApp.API.Data;
using WeddingApp.API.Models;
using WeddingApp.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using WeddingApp.API.Services;

namespace WeddingApp.API.Controllers
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            UserForDetailedDto user = await _service.Register(userForRegisterDto);

            if(user != null) 
                return CreatedAtRoute("GetUser", new {controller = "Users", id = user.Id}, user);
            else
                return BadRequest("Username already exists");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            Tuple<UserForListDto, string> toReturn;
            try {
                toReturn = await _service.Login(userForLoginDto);
            } catch (Exception e) {
                return BadRequest(e);
            }

            var user = toReturn.Item1;
            var token = toReturn.Item2;

            return Ok(new
            {
                token,
                user
            });
        }
    }
}