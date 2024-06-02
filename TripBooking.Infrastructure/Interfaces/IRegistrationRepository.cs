using TripBooking.Domain.Entities;

namespace TripBooking.Infrastructure.Interfaces
{
    public interface IRegistrationRepository
    {
        Task AddAsync(Registration registration);

        Task<bool> IsEmailRegisteredForTripAsync(string email, int tripId);
    }
}
