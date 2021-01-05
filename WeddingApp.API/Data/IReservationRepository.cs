using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeddingApp.API.Models;

namespace WeddingApp.API.Data
{
    public interface IReservationRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<Reservation> GetReservation(int id);
        Task<IEnumerable<Reservation>> GetReservationsForUser(int id);
        Task<IEnumerable<Reservation>> GetReservationsForPlace(int id);
        Task<bool> CheckReservationByDate(DateTime date, int placeId);
        Task<bool> SaveAll();
    }
}