using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WeddingApp.API.Data;
using WeddingApp.API.Dtos;
using WeddingApp.API.Helpers;
using WeddingApp.API.Models;

namespace WeddingApp.API.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public MessagesService(IUserRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<Message> GetMessage(int id)
        {
            var messageFromRepo = await _repo.GetMessage(id);
            return messageFromRepo;
        }

        public async Task<Tuple<PagedList<Message>, IEnumerable<MessageToReturnDto>>> GetMessagesForUser(int userId, MessageParams messageParams)
        {
            messageParams.UserId = userId;
            var messagesFromRepo = await _repo.GetMessagesForUser(messageParams);
            var messages = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);
            
            return Tuple.Create(messagesFromRepo, messages);
        }

        public async Task <IEnumerable<MessageToReturnDto>> GetMessageThread(int userId, int recipientId) {
            
            var messageFromRepo = await _repo.GetMessageThread(userId, recipientId);

            var messageThread = _mapper.Map<IEnumerable<MessageToReturnDto>>(messageFromRepo);

            return messageThread;
        }

        public async Task<MessageToReturnDto> CreateMessage(int userId, MessageForCreationDto messageForCreationDto)
        {
            var sender = await _repo.GetUser(userId);

            messageForCreationDto.SenderId = userId;

            var recipient = await _repo.GetUser(messageForCreationDto.RecipientId);

            if (recipient == null)
                throw new System.Exception("Could not find user");

            var message = _mapper.Map<Message>(messageForCreationDto);

            _repo.Add(message);

            if (await _repo.SaveAll())
            {
                //var messageToReturn = _mapper.Map<MessageForCreationDto>(message);
                var messageToReturn = _mapper.Map<MessageToReturnDto>(message);
                    return messageToReturn;
            }

            throw new System.Exception("Creating the message field on save");
        }

        public async Task<bool> DeleteMessage(int id, int userId) {
            var messageFromRepo = await _repo.GetMessage(id);

            if (messageFromRepo.SenderId == userId)
                messageFromRepo.SenderDeleted = true;

            if (messageFromRepo.RecipientId == userId)
                messageFromRepo.RecipientDeleted = true;

            if (messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted)
                _repo.Delete(messageFromRepo);

            if (await _repo.SaveAll())
                return true;

            throw new System.Exception("Error deleted message");
        }

        public async Task<bool> MarkMessageAsRead(int userId, int id) {
            var message = await _repo.GetMessage(id);

            if (message.RecipientId != userId)
                throw new System.Exception("Unauthorized");

            message.IsRead = true;
            message.DateRead = DateTime.Now;

            await _repo.SaveAll();

            return true;
        }
    }
}