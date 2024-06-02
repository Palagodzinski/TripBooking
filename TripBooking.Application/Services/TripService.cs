using TripBooking.Application.Exceptions;
using TripBooking.Application.Interfaces;
using TripBooking.Domain.Entities;
using TripBooking.Infrastructure.Interfaces;

namespace TripBooking.Application.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;

        public TripService(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        public async Task<int?> CreateTripAsync(Trip trip)
        {
            var existingTripName = await _tripRepository.GetByNameAsync(trip.Name);
            if (existingTripName != null)
            {
                throw new TripNameAlreadyExistsException($"Given trip name = {trip.Name} already exists");
            }

            var createdTripId = await _tripRepository.AddAsync(trip);

            return createdTripId;
        }

        public async Task UpdateTripAsync(int id, Trip tripForUpdate)
        {
            var trip = await _tripRepository.GetByIdAsync(id);
            if (trip == null)
            {
                throw new TripNotFoundException($"There is no existing trip with given id = {id}");
            }
            
            await _tripRepository.UpdateAsync(tripForUpdate, trip);
        }

        public async Task DeleteTripAsync(int id)
        {
            var trip = await _tripRepository.GetByIdAsync(id);
            if (trip == null)
            {
                throw new TripNotFoundException($"There is no existing trip with given id = {id}");
            }

            await _tripRepository.DeleteAsync(trip);
        }

        public async Task<Trip?> GetTripByIdAsync(int id) =>
            await _tripRepository.GetByIdAsync(id);

        public async Task<Trip?> GetFirstOrDefaultTripAsync() => 
            await _tripRepository.GetFirstOrDefaultAsync();

        public async Task<IEnumerable<Trip>> GetTripsByCountryAsync(string country) =>
            await _tripRepository.GetByCountryAsync(country);

        public async Task<IEnumerable<Trip>> GetAllTripsAsync() =>
            await _tripRepository.GetAllAsync();
        
    }
}
