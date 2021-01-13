using System.Collections.Generic;
using System.Threading.Tasks;
using WeddingApp.API.Dtos;

namespace WeddingApp.API.Services
{
    public interface IReservationsService
    {
         Task<bool> AddReservation(int userId, ReservationForCreationDto reservationForCreationDto);
         Task<IEnumerable<ReservationToReturnDto>> GetReservationsForUser(int userId);
         Task<IEnumerable<ReservationToReturnDto>> GetReservationsForPlace(int userId, int placeId);
         Task<bool> DeleteReservation(int userId, int id);
         Task<bool> PayReservation(int id);
    }
}