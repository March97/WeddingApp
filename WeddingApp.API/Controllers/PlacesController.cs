using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using WeddingApp.API.Data;
using WeddingApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeddingApp.API.Models;
using System.Collections.Generic;
using System;

namespace WeddingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/places")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private readonly IPlaceRepository _repo;
        private readonly IDatingRepository _repoDating;
        private readonly IMapper _mapper;
        public PlacesController(IPlaceRepository repo, IDatingRepository repoDating, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
            _repoDating = repoDating;
        }

        [HttpPost]
        public async Task<IActionResult> AddPlaceForUser(int userId, 
            PlaceForCreationDto placeForCreationDto) 
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repoDating.GetUser(userId);

            var placeToCreate = _mapper.Map<Place>(placeForCreationDto);

            userFromRepo.Places.Add(placeToCreate);

            if (await _repoDating.SaveAll())
            {
                var placeToReturn = _mapper.Map<PlaceForReturnDto>(placeToCreate);
                return Ok();
            }

            return BadRequest("Could not add the place");
        }

        [HttpGet]
        public async Task<IActionResult> GetPlacesForUser(int userId)
        {
            var places = await _repo.GetPlacesForUser(userId);

            var placesToReturn = _mapper.Map<IEnumerable<PlacesForListDto>>(places);

            return Ok(placesToReturn);
        }

        [HttpGet("{id}", Name="GetPlaces")]
        public async Task<IActionResult> GetPlace(int id)
        {
            var place = await _repo.GetPlace(id);

            var placeToReturn = _mapper.Map<PlaceForReturnDto>(place);

            return Ok(placeToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlace(int userId, int id, PlaceForUpdateDto placeForUpdateDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var placeFromRepo = await _repo.GetPlace(id);

            _mapper.Map(placeForUpdateDto, placeFromRepo);

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception($"Updating place {id} failed on save");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlace(int userId, int id) 
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var placeFromRepo = await _repo.GetPlace(id);
            _repo.Delete(placeFromRepo);

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest($"Failed to delete the place id: {id}");
        }
    }
}