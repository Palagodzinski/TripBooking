using Microsoft.EntityFrameworkCore;
using TripBooking.Domain.Entities;
using TripBooking.Infrastructure.Data;
using TripBooking.Infrastructure.Interfaces;

namespace TripBooking.Infrastructure.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly AppDbContext _context;

        public TripRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Trip trip)
        {
            trip.Id = CreateNewTripId();

            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();

            return trip.Id;
        }

        public async Task UpdateAsync(Trip tripForUpdate, Trip trip)
        {
            trip.Country = tripForUpdate.Country;
            trip.Name = tripForUpdate.Name;
            trip.Description = tripForUpdate.Description;
            trip.StartDate = tripForUpdate.StartDate;
            trip.NumberOfSeats = tripForUpdate.NumberOfSeats;

            _context.Trips.Update(trip);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Trip trip)
        {
            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Trip>> GetByCountryAsync(string country) =>
            await _context.Trips.Where(x => x.Country.Equals(country)).ToListAsync();

        public async Task<Trip?> GetByIdAsync(int id) =>
            await _context.Trips
            .Include(x => x.Registrations)
            .SingleOrDefaultAsync(x => x.Id == id);

        public async Task<Trip?> GetFirstOrDefaultAsync() =>
            await _context.Trips
            .Include(x => x.Registrations)
            .FirstOrDefaultAsync();

        public async Task<IEnumerable<Trip>> GetAllAsync() =>
            await _context.Trips.ToListAsync();

        public async Task<Trip?> GetByNameAsync(string name) =>
            await _context.Trips.SingleOrDefaultAsync(x => x.Name.Equals(name));
        
        private int CreateNewTripId()
        {
            var lastTripId = _context.Trips.Max(x => (int?)x.Id) ?? 0;

            return lastTripId + 1;
        }
    }
}
