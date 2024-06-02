using TripBooking.Domain.Entities;

namespace TripBooking.Application.Interfaces
{
    public interface IRegistrationService
    {
        Task RegisterAsync(Registration registration);
    }
}
