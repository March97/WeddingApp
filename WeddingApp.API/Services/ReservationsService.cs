using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using WeddingApp.API.Data;
using WeddingApp.API.Dtos;
using WeddingApp.API.Models;

namespace WeddingApp.API.Services
{
    public class ReservationsService : IReservationsService
    {
        private readonly IReservationRepository _repoRes;
        private readonly IUserRepository _repoDating;
        private readonly IPlaceRepository _repoPlace;
        private readonly IMapper _mapper;

        public ReservationsService(IReservationRepository repoRes, IUserRepository repoDating, IPlaceRepository repoPlace, IMapper mapper)
        {
            _repoDating = repoDating;
            _repoRes = repoRes;
            _mapper = mapper;
            _repoPlace = repoPlace;
        }

        public async Task<bool> AddReservation(int userId, ReservationForCreationDto reservationForCreationDto) 
        {
            var resToCreate = _mapper.Map<Reservation>(reservationForCreationDto);
            var place = await _repoPlace.GetPlace(resToCreate.PlaceId);

            if (place.Capacity < resToCreate.AmountOfGuests) {
                throw new System.Exception("Too many guests for this place.");
            } else
            if (await _repoRes.CheckReservationByDate(resToCreate.Date, resToCreate.PlaceId)) {
                throw new System.Exception($"Date {resToCreate.Date.Date} is already booked for this place.");
            } else
            if (resToCreate.Date.Date < DateTime.Now) {
                throw new System.Exception("You have to choose minimum tomorrow date.");
            }
            else {
                resToCreate.Cost = resToCreate.AmountOfGuests * place.Price;
                resToCreate.Date = resToCreate.Date;
                _repoRes.Add(resToCreate);

                if (await _repoRes.SaveAll())
                {
                    return true;
                }
                throw new System.Exception("Creating the reservation field on save");
            }
        }

        public async Task<IEnumerable<ReservationToReturnDto>> GetReservationsForUser(int userId) 
        {
            var reservationsFromRepo = await _repoRes.GetReservationsForUser(userId);

            var reservations = _mapper.Map<IEnumerable<ReservationToReturnDto>>(reservationsFromRepo);

            return reservations;
        }

        public async Task<IEnumerable<ReservationToReturnDto>> GetReservationsForPlace(int userId, int placeId)
        {
            var place = await _repoPlace.GetPlace(placeId);

            if (place.UserId != userId)
                throw new System.Exception("Unathorized");

            var reservationsFromRepo = await _repoRes.GetReservationsForPlace(placeId);

            var reservations = _mapper.Map<IEnumerable<ReservationToReturnDto>>(reservationsFromRepo);

            return reservations;
        }

        public async Task<bool> DeleteReservation(int userId, int id) 
        {
            var reservationFromRepo = await _repoRes.GetReservation(id);

            if (userId != reservationFromRepo.UserId)
                throw new System.Exception("Unathorized");

            _repoRes.Delete(reservationFromRepo);

            if (await _repoRes.SaveAll())
                return true;

            throw new System.Exception($"Failed to delete the reservation id: {id}");
        }

        public async Task<bool> PayReservation(int id)
        {
            await _repoRes.PayReservation(id);

            if (await _repoRes.SaveAll())
                return true;

            throw new System.Exception($"Failed to paid the reservation id: {id}");
        }
    }
}