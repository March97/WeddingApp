using System;

namespace WeddingApp.API.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public Place Place { get; set; }
        public int PlaceId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public int AmountOfGuests { get; set; }
        public int Cost { get; set; }
        public string Comments { get; set; }
    }
}