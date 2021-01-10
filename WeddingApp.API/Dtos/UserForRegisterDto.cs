using System;
using System.ComponentModel.DataAnnotations;

namespace WeddingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(24, MinimumLength = 8, ErrorMessage = "You must specified password between 8 and 24")]
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Profession { get; set; }
        [Required]
        public string KnownAs { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }


        public UserForRegisterDto()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
        
    }
}