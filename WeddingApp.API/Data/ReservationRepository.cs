using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeddingApp.API.Models;

namespace WeddingApp.API.Data
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly DataContext _context;

        public ReservationRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Reservation> GetReservation(int id) {
            var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);
            return reservation;
        }

        public async Task<IEnumerable<Reservation>> GetReservationsForUser(int id) {
            var reservations = await _context.Reservations.Where(u => u.UserId == id).ToListAsync();
            return reservations;
        }

        public async Task<IEnumerable<Reservation>> GetReservationsForPlace(int id) {
            var reservations = await _context.Reservations.Where(u => u.PlaceId == id).ToListAsync();
            return reservations;
        }

        public async Task<bool> CheckReservationByDate(DateTime date, int placeId) {
            var reservations = await _context.Reservations.Where(u => u.PlaceId == placeId).ToListAsync();
            reservations = reservations.FindAll(u => u.Date.Year == date.Year);
            reservations = reservations.FindAll(u => u.Date.Month == date.Month);
            reservations = reservations.FindAll(u => u.Date.Day == date.Day);

            if (reservations.Any()) {
                return true;
            } else
            return false;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}