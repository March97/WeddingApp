using System;
using System.Security.Claims;
using System.Threading.Tasks;
using WeddingApp.API.Dtos;
using WeddingApp.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeddingApp.API.Services;

namespace WeddingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        // private readonly IUserRepository _repo;
        // private readonly IMapper _mapper;
        private readonly IMessagesService _service;

        public MessagesController(IMessagesService service)
        {
            _service = service;
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _service.GetMessage(id);

            if (messageFromRepo == null)
                return NotFound();

            return Ok(messageFromRepo);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId, [FromQuery]MessageParams messageParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var t = await _service.GetMessagesForUser(userId, messageParams);
            var messages = t.Item2;

            Response.AddPagination(t.Item1.CurrentPage, t.Item1.PageSize, 
                t.Item1.TotalCount, t.Item1.TotalPages);

            return Ok(messages);
        }

        [HttpGet("thread/{recipientId}")]
        public async Task <IActionResult> GetMessageThread(int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageThread = await _service.GetMessageThread(userId, recipientId);
            
            return Ok(messageThread);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, 
            MessageForCreationDto messageForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            MessageToReturnDto messageToReturn;

            try {
                messageToReturn = await _service.CreateMessage(userId, messageForCreationDto);
            } catch (Exception e) {
                return BadRequest("" + e);
            }

            return CreatedAtRoute("GetMessage", 
                        new {userId, id = messageToReturn.Id}, messageToReturn);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            try {
                var a = await _service.DeleteMessage(id, userId);
            } catch (Exception e) {
                return BadRequest("" + e);
            }

            return NoContent();
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            try {
                var a = await _service.MarkMessageAsRead(userId, id);
            } catch (Exception e) {
                return BadRequest("" + e);
            }

            return NoContent();
        }
    }
}