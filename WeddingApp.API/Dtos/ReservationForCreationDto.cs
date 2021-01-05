using System;

namespace WeddingApp.API.Dtos
{
    public class ReservationForCreationDto
    {
        public int PlaceId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public int AmountOfGuests { get; set; }
        public string Comments { get; set; }
    }
}