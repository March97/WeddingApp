using System;

namespace WeddingApp.API.Dtos
{
    public class PaymentDto {
        public int amountOfGuests { get; set; }
        public string comments { get; set; }
        public int cost { get; set; }
        public DateTime date { get; set; }
        public int id { get; set; }
        public int placeId { get; set; }
        public string placeName { get; set; }
        public int userId { get; set; }
        public string userName { get; set; }
        
    }
}