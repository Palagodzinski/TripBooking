using System.ComponentModel.DataAnnotations;

namespace TripBooking.API.Models
{
    public class TripDto
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(20)]
        public string Country { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Range(1, 100)]
        public int NumberOfSeats { get; set; }
    }
}
