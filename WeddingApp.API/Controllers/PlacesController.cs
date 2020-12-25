using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using WeddingApp.API.Data;
using WeddingApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeddingApp.API.Models;

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

            // placeToCreate.UserId = userId;

            // if (userFromRepo.Places == null) {
            //     userFromRepo.Places = new ICollection();
            // }
            userFromRepo.Places.Add(placeToCreate);

            if (await _repoDating.SaveAll())
            {
                var placeToReturn = _mapper.Map<PlaceForReturnDto>(placeToCreate);
                // return CreatedAtRoute("GetPlace", new { userId = userId, id = placeToCreate.Id}, placeToReturn);
                return Ok();
            }

            return BadRequest("Could not add the place");
        }

        [HttpGet]
        public async Task<IActionResult> GetPlaces()
        {
            var places = await _repo.GetPlaces();

            //var placesToReturn = _mapper.Map<IEnumerable<PlacesForListDto>>(places);

            return Ok(places);
        }

        [HttpGet("{id}", Name="GetPlaces")]
        public async Task<IActionResult> GetPlace(int id)
        {
            var place = await _repo.GetPlace(id);

            //var placeToReturn = _mapper.Map<PlaceFor>(user);

            return Ok(place);
        }

        // [HttpPut("{id}")]
        // public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        // {
        //     if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        //         return Unauthorized();

        //     var userFromRepo = await _repo.GetUser(id);

        //     _mapper.Map(userForUpdateDto, userFromRepo);

        //     if (await _repo.SaveAll())
        //         return NoContent();

        //     throw new Exception($"Updating user {id} failed on save");
        // }
    }
}