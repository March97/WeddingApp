using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using WeddingApp.API.Data;
using WeddingApp.API.Dtos;
using WeddingApp.API.Helpers;
using WeddingApp.API.Models;

namespace WeddingApp.API.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        public UsersService(IUserRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<Tuple<IEnumerable<UserForListDto>, PagedList<User>>> GetUsers(UserParams userParams, int userId)
        {
            var userFromRepo = await _repo.GetUser(userId);

            userParams.UserId = userId;

            var users = await _repo.GetUsers(userParams);

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            return Tuple.Create(usersToReturn, users);
        }

        public async Task<UserForDetailedDto> GetUser(int id)
        {
            var user = await _repo.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return userToReturn;
        }

        public async Task<bool> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            var userFromRepo = await _repo.GetUser(id);

            _mapper.Map(userForUpdateDto, userFromRepo);

            if (await _repo.SaveAll())
                return true;

            throw new Exception($"Updating user {id} failed on save");
        }

        public async Task<bool> LikeUser(int id, int recipientId)
        {
            var like = await _repo.GetLike(id, recipientId);

            if (like != null) 
                throw new Exception("You already liked this user");

            if (await _repo.GetUser(recipientId) == null)
                throw new Exception("Not found");

            like = new Like
            {
                LikerId = id,
                LikeeId = recipientId
            };

            _repo.Add<Like>(like);

            if (await _repo.SaveAll())
                return true;

            throw new Exception("Failed to like user");
        }
    }
}