using System.Security.Claims;
using System.Threading.Tasks;
using WeddingApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WeddingApp.API.Services;
using System;

namespace WeddingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotosService _service;

        public PhotosController(IPhotosService service)
        {
            _service = service;
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id) 
        {
            PhotoForReturnDto photo;
            try {
                photo = await _service.GetPhoto(id);
            } catch (Exception e) {
                return BadRequest(e);
            }
            return Ok(photo);
        } 

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, 
            [FromForm]PhotoForCreationDto photoForCreationDto) 
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            PhotoForReturnDto photoToReturn;

            try {
                photoToReturn = await _service.AddPhotoForUser(userId, photoForCreationDto);
            } catch (Exception e) {
                return BadRequest("Could not add the photo " + e);
            }
            return CreatedAtRoute("GetPhoto", new { userId = userId, id = photoToReturn.Id}, photoToReturn);       
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            bool result;

            try {
                result = await _service.SetMainPhoto(userId, id);
            } catch (Exception e) {
                return BadRequest(e);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id) 
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            bool result;
            try {
                result = await _service.DeletePhoto(userId, id);
            } catch (Exception e) {
                return BadRequest(e);
            }
            
            return Ok();
        }
    }
}