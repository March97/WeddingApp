using System.Threading.Tasks;
using WeddingApp.API.Dtos;

namespace WeddingApp.API.Services
{
    public interface IPhotosForPlaceService
    {
         Task<PhotoForReturnDto> GetPhoto(int id);
         Task<PhotoForReturnDto> AddPhotoForPlace(int userId, int placeId, PhotoForCreationDto photoForCreationDto);
         Task<bool> SetMainPhoto(int userId, int placeId, int id);
         Task<bool> DeletePhoto(int userId, int placeId, int id);
    }
}