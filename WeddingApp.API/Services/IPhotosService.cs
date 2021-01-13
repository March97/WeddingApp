using System.Threading.Tasks;
using WeddingApp.API.Dtos;

namespace WeddingApp.API.Services
{
    public interface IPhotosService
    {
         Task<PhotoForReturnDto> GetPhoto(int id);
         Task<PhotoForReturnDto> AddPhotoForUser(int userId, PhotoForCreationDto photoForCreationDto);
         Task<bool> SetMainPhoto(int userId, int id);
         Task<bool> DeletePhoto(int userId, int id);
    }
}