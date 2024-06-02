using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripBooking.Domain.Entities
{
    public class Registration
    {
        [Key]
        public int Id { get; set; }

        public string Email { get; set; }
        
        public int TripId { get; set; }

        [ForeignKey("TripId")]
        public virtual Trip Trip { get; set; }
    }
}
