using TripBooking.Domain.Entities;

namespace TripBooking.Infrastructure.Interfaces
{
    public interface ITripRepository
    {
        Task<int> AddAsync(Trip trip);

        Task UpdateAsync(Trip tripForUpdate, Trip trip);

        Task DeleteAsync(Trip trip);

        Task<Trip?> GetByIdAsync(int id);

        Task<IEnumerable<Trip>> GetAllAsync();

        Task<IEnumerable<Trip>> GetByCountryAsync(string country);

        Task<Trip?> GetFirstOrDefaultAsync();

        Task<Trip?> GetByNameAsync(string name);
    }
}
