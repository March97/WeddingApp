using System;

namespace WeddingApp.API.Dtos
{
    public class ReservationToReturnDto
    {
        public int Id { get; set; }
        public int PlaceId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public int AmountOfGuests { get; set; }
        public int Cost { get; set; }
        public string Comments { get; set; }
        public bool Paid { get; set; }
    }
}