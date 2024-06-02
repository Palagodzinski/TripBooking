using System.ComponentModel.DataAnnotations;

namespace TripBooking.Domain.Entities
{
    public class Trip
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public int NumberOfSeats { get; set; }

        public virtual IEnumerable<Registration> Registrations { get; set; }
    }
}
