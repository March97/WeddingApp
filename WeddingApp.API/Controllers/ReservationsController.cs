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
using WeddingApp.API.Services;

namespace WeddingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/reservations")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationsService _service;

        public ReservationsController(IReservationsService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddReservation(int userId, ReservationForCreationDto reservationForCreationDto) {
            
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            bool result;

            try {
                result = await _service.AddReservation(userId, reservationForCreationDto);
                return Ok();
            } catch (Exception e) {
                return BadRequest("" + e);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetReservationsForUser(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            IEnumerable<ReservationToReturnDto> reservations;
            
            try {
                reservations = await _service.GetReservationsForUser(userId);
                return Ok(reservations);
            } catch (Exception e) {
                return BadRequest("" + e);
            }
        }

        [HttpGet("places/{placeId}")]
        public async Task<IActionResult> GetReservationsForPlace(int userId, int placeId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            IEnumerable<ReservationToReturnDto> reservations;

            try {
                reservations = await _service.GetReservationsForPlace(userId, placeId);
                return Ok(reservations);
            } catch (Exception e) {
                return BadRequest("" + e);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int userId, int id) 
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            bool result;

            try {
                result = await _service.DeleteReservation(userId, id);
                return Ok();
            } catch (Exception e) {
                return BadRequest("" + e);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PayReservation(int id) 
        {
            bool result;

            try {
                result = await _service.PayReservation(id);
                return Ok();
            } catch (Exception e) {
                return BadRequest("" + e);
            }
        }
    }
}