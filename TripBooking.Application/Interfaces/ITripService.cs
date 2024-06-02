using TripBooking.Domain.Entities;

namespace TripBooking.Application.Interfaces
{
    public interface ITripService
    {
        Task<int?> CreateTripAsync(Trip trip);

        Task UpdateTripAsync(int id, Trip trip);

        Task DeleteTripAsync(int id);

        Task<IEnumerable<Trip>> GetAllTripsAsync();

        Task<IEnumerable<Trip>> GetTripsByCountryAsync(string country);

        Task<Trip?> GetFirstOrDefaultTripAsync();

        Task<Trip?> GetTripByIdAsync(int id);
    }
}
