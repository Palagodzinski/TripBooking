namespace TripBooking.API.Models
{
    public record TripSummaryDto(
        string Name,
        string Country,
        DateTime StartDate);
}
