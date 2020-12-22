using System;
using System.ComponentModel.DataAnnotations;

namespace WeddingApp.API.Dtos
{
    public class PlaceForCreationDto
    {
        [Required]
        public string Name { get; set; }

        public DateTime Created { get; set; }

        [Required]
        public string Country { get; set; }
        
        [Required]
        public string City { get; set; }

        [Required]
        public string Address { get; set; }

        public string Facilities { get; set; }

        [Required]
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