using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeddingApp.API.Dtos;
using WeddingApp.API.Helpers;
using WeddingApp.API.Models;

namespace WeddingApp.API.Services
{
    public interface IUsersService
    {
         Task<Tuple<IEnumerable<UserForListDto>, PagedList<User>>> GetUsers(UserParams userParams, int userId);
         Task<UserForDetailedDto> GetUser(int id);
         Task<bool> UpdateUser(int id, UserForUpdateDto userForUpdateDto);
         Task<bool> LikeUser(int id, int recipientId);
    }
}