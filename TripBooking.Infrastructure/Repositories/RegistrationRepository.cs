using Microsoft.EntityFrameworkCore;
using TripBooking.Domain.Entities;
using TripBooking.Infrastructure.Data;
using TripBooking.Infrastructure.Interfaces;

namespace TripBooking.Infrastructure.Repositories
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly AppDbContext _context;

        public RegistrationRepository(AppDbContext context)
        {
            _context = context;
                
        }
        public async Task AddAsync(Registration registration)
        {
            registration.Id = CreateNewRegistrationId();

            _context.Add(registration);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsEmailRegisteredForTripAsync(string email, int tripId) =>
            await _context.Registrations.AnyAsync(x => x.Email.Equals(email) && x.TripId == tripId);

        private int CreateNewRegistrationId()
        {
            var lastRegistrationId = _context.Registrations.Max(x => (int?)x.Id) ?? 0;

            return lastRegistrationId + 1;
        }
    }
}
