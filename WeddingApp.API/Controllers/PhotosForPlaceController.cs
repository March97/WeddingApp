using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using WeddingApp.API.Data;
using WeddingApp.API.Dtos;
using WeddingApp.API.Helpers;
using WeddingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WeddingApp.API.Services;
using System;

namespace WeddingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/places/{placeId}/photos")]
    [ApiController]
    public class PhotosForPlaceController : ControllerBase
    {
        private readonly IPhotosForPlaceService _service;
        public PhotosForPlaceController(IPhotosForPlaceService service)
        {
            _service = service;
        }

        [HttpGet("{id}", Name = "GetPhotoForPlace")]
        public async Task<IActionResult> GetPhoto(int id) 
        {
            PhotoForReturnDto photo;
            try {
                photo = await _service.GetPhoto(id);
            } catch (Exception e) {
                return BadRequest("" + e);
            }

            return Ok(photo);
        } 

        [HttpPost]
        public async Task<IActionResult> AddPhotoForPlace(int userId, int placeId,
            [FromForm]PhotoForCreationDto photoForCreationDto) 
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            PhotoForReturnDto photo;
            try {
                photo = await _service.AddPhotoForPlace(userId, placeId, photoForCreationDto);
            } catch (Exception e) {
                return BadRequest("Could not add the photo" + e);
            }
            
            return CreatedAtRoute("GetPhotoForPlace", new {userId = userId, placeId = placeId, id = photo.Id}, photo);
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int placeId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            bool result;

            try {
                result = await _service.SetMainPhoto(userId, placeId, id);
            } catch (Exception e) {
                return BadRequest("Could not set photo to main " + e);
            }
                
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int placeId, int id) 
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            bool result;

            try {
                result = await _service.SetMainPhoto(userId, placeId, id);
            } catch (Exception e) {
                return BadRequest("Failed to delete the photo " + e);
            }
            return Ok();
        }
    }
}