using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeddingApp.API.Dtos;
using WeddingApp.API.Helpers;
using WeddingApp.API.Models;

namespace WeddingApp.API.Services
{
    public interface IPlacesService
    {
         Task<PlaceForReturnDto> AddPlaceForUser(int userId, PlaceForCreationDto placeForCreationDto);
         Task<IEnumerable<PlacesForListDto>> GetPlacesForUser(int userId);
         Task<Tuple<IEnumerable<PlacesForListDto>, PagedList<Place>>> GetPlaces(PlaceParams placeParams);
         Task<Place> GetPlace(int id);
         Task<bool> UpdatePlace(int userId, int id, PlaceForUpdateDto placeForUpdateDto);
         Task<bool> DeletePlace(int userId, int id);
    }
}