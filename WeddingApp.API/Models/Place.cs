using System;
using System.Collections.Generic;

namespace WeddingApp.API.Models
{
    public class Place
    {
        public int Id { get; set; }

        public User Owner { get; set; }

        public int OwnerId { get; set; }

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
        
        public ICollection<Photo> Photos { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}