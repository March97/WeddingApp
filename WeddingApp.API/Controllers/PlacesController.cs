using System.Security.Claims;
using System.Threading.Tasks;
using WeddingApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeddingApp.API.Models;
using System.Collections.Generic;
using System;
using WeddingApp.API.Helpers;
using WeddingApp.API.Services;

namespace WeddingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/places")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private readonly IPlacesService _service;
        public PlacesController(IPlacesService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddPlaceForUser(int userId, 
            PlaceForCreationDto placeForCreationDto) 
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            PlaceForReturnDto placeToReturn;
            try {
                placeToReturn = await _service.AddPlaceForUser(userId, placeForCreationDto);
            } catch (Exception e) {
                return BadRequest("" + e);
            }
            
            if (placeToReturn != null)
                return Ok();

            return BadRequest("Could not add the place");
        }

        [HttpGet]
        public async Task<IActionResult> GetPlacesForUser(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            IEnumerable<PlacesForListDto> places;

            try {
                places = await _service.GetPlacesForUser(userId);
            } catch (Exception e) {
                return BadRequest("" + e);
            }

            return Ok(places);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetPlaces([FromQuery]PlaceParams placeParams)
        {
            PagedList<Place> places;

            IEnumerable<PlacesForListDto> placesToReturn;

            try {
                var t = await _service.GetPlaces(placeParams);
                placesToReturn = t.Item1;
                places = t.Item2;
            } catch (Exception e) {
                return BadRequest("" + e);
            }

            Response.AddPagination(places.CurrentPage, places.PageSize, 
                places.TotalCount, places.TotalPages);

            return Ok(placesToReturn);
        }

        [HttpGet("{id}", Name="GetPlaces")]
        public async Task<IActionResult> GetPlace(int id)
        {
            try {
                return Ok(await _service.GetPlace(id));
            } catch (Exception e) {
                return BadRequest("" + e);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlace(int userId, int id, PlaceForUpdateDto placeForUpdateDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            try {
                var result = await _service.UpdatePlace(userId, id, placeForUpdateDto);
                return NoContent();
            } catch (Exception e) {
                return BadRequest("" + e);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlace(int userId, int id) 
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            try {
                var result = await _service.DeletePlace(userId, id);
                return Ok();
            } catch (Exception e) {
                return BadRequest("" + e);
            }
        }
    }
}