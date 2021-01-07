using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeddingApp.API.Data;
using WeddingApp.API.Dtos;
using WeddingApp.API.Helpers;
using WeddingApp.API.Models;

namespace WeddingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/reservations")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationRepository _repoRes;
        private readonly IDatingRepository _repoDating;
        private readonly IPlaceRepository _repoPlace;
        private readonly IMapper _mapper;

        public ReservationsController(IReservationRepository repoRes, IDatingRepository repoDating, IPlaceRepository repoPlace, IMapper mapper)
        {
            _repoDating = repoDating;
            _repoRes = repoRes;
            _mapper = mapper;
            _repoPlace = repoPlace;
        }

        [HttpPost]
        public async Task<IActionResult> AddReservation(int userId, ReservationForCreationDto reservationForCreationDto) {
            
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var resToCreate = _mapper.Map<Reservation>(reservationForCreationDto);
            var place = await _repoPlace.GetPlace(resToCreate.PlaceId);

            if (place.Capacity < resToCreate.AmountOfGuests) {
                return BadRequest("Too many guests for this place.");
            } else
            if (await _repoRes.CheckReservationByDate(resToCreate.Date, resToCreate.PlaceId)) {
                return BadRequest($"Date {resToCreate.Date.Date} is already booked for this place.");
            } else
            if (resToCreate.Date.Date < DateTime.Now) {
                return BadRequest("You have to choose minimum tomorrow date.");
            }
            else {
                resToCreate.Cost = resToCreate.AmountOfGuests * place.Price;
                _repoRes.Add(resToCreate);

                if (await _repoRes.SaveAll())
                {
                    return Ok();
                }
                throw new System.Exception("Creating the reservation field on save");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var reservationsFromRepo = await _repoRes.GetReservationsForUser(userId);

            var reservations = _mapper.Map<IEnumerable<ReservationToReturnDto>>(reservationsFromRepo);

            return Ok(reservations);
        }

        [HttpGet("places/{placeId}")]
        public async Task<IActionResult> GetMessagesForPlace(int userId, int placeId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var place = await _repoPlace.GetPlace(placeId);

            if (place.UserId != userId)
                return Unauthorized();

            var reservationsFromRepo = await _repoRes.GetReservationsForPlace(placeId);

            var reservations = _mapper.Map<IEnumerable<ReservationToReturnDto>>(reservationsFromRepo);

            return Ok(reservations);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int userId, int id) 
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var reservationFromRepo = await _repoRes.GetReservation(id);

            if (userId != reservationFromRepo.UserId)
                return Unauthorized();

            _repoRes.Delete(reservationFromRepo);

            if (await _repoRes.SaveAll())
                return Ok();

            return BadRequest($"Failed to delete the reservation id: {id}");
        }
    }
}