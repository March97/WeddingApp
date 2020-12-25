using System;

namespace WeddingApp.API.Dtos
{
    public class PlaceForCreationDto
    {
        public string Name { get; set; }

        public DateTime Created { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Facilities { get; set; }

        public int Capacity { get; set; }

        public int Price { get; set; }

        public string InPrice { get; set; }

        public string Bonuses { get; set; }

        public string Description { get; set; }

        public PlaceForCreationDto()
        {
            Created = DateTime.Now;
        }
    }
}