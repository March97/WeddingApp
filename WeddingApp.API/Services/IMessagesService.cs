using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeddingApp.API.Dtos;
using WeddingApp.API.Helpers;
using WeddingApp.API.Models;

namespace WeddingApp.API.Services
{
    public interface IMessagesService
    {
        Task<Message> GetMessage(int id);
        Task<Tuple<PagedList<Message>, IEnumerable<MessageToReturnDto>>> GetMessagesForUser(int userId, MessageParams messageParams);
        Task <IEnumerable<MessageToReturnDto>> GetMessageThread(int userId, int recipientId);
        Task<MessageToReturnDto> CreateMessage(int userId,  MessageForCreationDto messageForCreationDto);
        Task<bool> DeleteMessage(int id, int userId);
        Task<bool> MarkMessageAsRead(int userId, int id);
    }
}