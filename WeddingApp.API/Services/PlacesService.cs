using System.Threading.Tasks;
using WeddingApp.API.Dtos;
using AutoMapper;
using WeddingApp.API.Data;
using WeddingApp.API.Models;
using System.Collections.Generic;
using System;
using WeddingApp.API.Helpers;

namespace WeddingApp.API.Services
{
    public class PlacesService : IPlacesService
    {
        private readonly IPlaceRepository _repo;
        private readonly IUserRepository _repoUser;
        private readonly IMapper _mapper;
        public PlacesService(IPlaceRepository repo, IUserRepository repoUser, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
            _repoUser = repoUser;
        }
        public async Task<PlaceForReturnDto> AddPlaceForUser(int userId, PlaceForCreationDto placeForCreationDto)
        {
            var userFromRepo = await _repoUser.GetUser(userId);

            var placeToCreate = _mapper.Map<Place>(placeForCreationDto);

            userFromRepo.Places.Add(placeToCreate);

            if (await _repoUser.SaveAll())
            {
                var placeToReturn = _mapper.Map<PlaceForReturnDto>(placeToCreate);
                return placeToReturn;
            }

            throw new System.Exception("Could not add the place");
        }

        public async Task<IEnumerable<PlacesForListDto>> GetPlacesForUser(int userId) 
        {
            var places = await _repo.GetPlacesForUser(userId);

            var placesToReturn = _mapper.Map<IEnumerable<PlacesForListDto>>(places);

            return placesToReturn;
        }

        public async Task<Tuple<IEnumerable<PlacesForListDto>, PagedList<Place>>> GetPlaces(PlaceParams placeParams)
        {
            var places = await _repo.GetPlaces(placeParams);

            var placesToReturn = _mapper.Map<IEnumerable<PlacesForListDto>>(places);

            return Tuple.Create(placesToReturn, places);
        }

        public async Task<Place> GetPlace(int id)
        {
            return await _repo.GetPlace(id);
        }

        public async Task<bool> UpdatePlace(int userId, int id, PlaceForUpdateDto placeForUpdateDto)
        {
            var placeFromRepo = await _repo.GetPlace(id);

            _mapper.Map(placeForUpdateDto, placeFromRepo);

            if (await _repo.SaveAll())
                return true;
            
            throw new System.Exception($"Updating place {id} failed on save");
        }

        public async Task<bool> DeletePlace(int userId, int id) 
        {
            var placeFromRepo = await _repo.GetPlace(id);

            if (userId != placeFromRepo.UserId)
                throw new System.Exception("Unauthorized");

            _repo.Delete(placeFromRepo);

            if (await _repo.SaveAll())
                return true;

            throw new System.Exception($"Failed to delete the place id: {id}");
        }
    }
}