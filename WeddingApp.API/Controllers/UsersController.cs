using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using WeddingApp.API.Data;
using WeddingApp.API.Dtos;
using WeddingApp.API.Helpers;
using WeddingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeddingApp.API.Services;

namespace WeddingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _service;
        public UsersController(IUsersService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            Tuple<IEnumerable<UserForListDto>, PagedList<User>> t;
            try {
                t = await _service.GetUsers(userParams, currentUserId);
            } catch (Exception e) {
                return BadRequest("" + e);
            }

            Response.AddPagination(t.Item2.CurrentPage, t.Item2.PageSize, 
                t.Item2.TotalCount, t.Item2.TotalPages);

            var usersToReturn = t.Item1;

            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name="GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            UserForDetailedDto userToReturn;
            try {
                userToReturn = await _service.GetUser(id);
            } catch (Exception e) {
                return BadRequest("" + e);
            }
            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            try {
                var result = await _service.UpdateUser(id, userForUpdateDto);
                return NoContent();
            } catch (Exception e) {
                return BadRequest("" + e);
            }
        }

        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            try {
                var result = await _service.LikeUser(id, recipientId);
                return Ok();
            } catch (Exception e) {
                return BadRequest("" + e);
            }
        }
    }
}