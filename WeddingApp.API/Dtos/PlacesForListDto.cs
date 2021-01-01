using System;

namespace WeddingApp.API.Dtos
{
    public class PlacesForListDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; } 

        public string PhotoUrl { get; set; }
    }
}