using System.Collections.Generic;
using System.Threading.Tasks;
using WeddingApp.API.Helpers;
using WeddingApp.API.Models;

namespace WeddingApp.API.Data
{
    public interface IPlaceRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity)  where T: class;
        Task<bool> SaveAll();
        Task<PagedList<Place>> GetPlaces(PlaceParams placeParams);
        Task<Place> GetPlace(int id);
        Task<PhotoForPlace> GetPhoto(int id);
        Task<PhotoForPlace> GetMainPhotoForPlace(int placeId);

        Task<IEnumerable<Place>> GetPlacesForUser(int userId);
    }
}