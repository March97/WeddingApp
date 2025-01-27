using System;
using System.Collections.Generic;

namespace WeddingApp.API.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Gender { get; set; }

        public string Profession { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string KnownAs { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public string Introduction { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public ICollection<Photo> Photos { get; set; }

        public ICollection<Like> Likers { get; set; }

        public ICollection<Like> Likees { get; set; }

        public ICollection<Message> MessagesSent { get; set; }

        public ICollection<Message> MessagesReceived { get; set; }
        
        public ICollection<Place> Places { get; set; }
        
        public ICollection<Reservation> Reservations { get; set; }

        public User()
        {
            Photos = new HashSet<Photo>();
            Likers = new HashSet<Like>();
            Likees = new HashSet<Like>();
            MessagesSent = new HashSet<Message>();
            MessagesReceived = new HashSet<Message>();
            Places = new HashSet<Place>();
            Reservations = new HashSet<Reservation>();
        }
    }
}