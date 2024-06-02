using System.ComponentModel.DataAnnotations;

namespace TripBooking.API.Models
{
    public class RegistrationDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public int TripId { get; set; }
    }
}
