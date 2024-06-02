using TripBooking.Application.Exceptions;
using TripBooking.Application.Interfaces;
using TripBooking.Domain.Entities;
using TripBooking.Infrastructure.Interfaces;

namespace TripBooking.Application.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly ITripService _tripService;

        public RegistrationService(IRegistrationRepository registrationRepository, ITripService tripService)
        {
            _registrationRepository = registrationRepository;
            _tripService = tripService;
        }

        public async Task RegisterAsync(Registration registration)
        {
            var trip = await _tripService.GetTripByIdAsync(registration.TripId);
            if (trip  == null) 
            {
                throw new TripNotFoundException($"There is no existing trip with given id = {registration.TripId}");
            }

            var existingRegistrationEmail = await _registrationRepository.IsEmailRegisteredForTripAsync(registration.Email, registration.TripId);
            if (existingRegistrationEmail)
            {
                throw new EmailAlreadyRegisteredForTripException($"Email = {registration.Email} is already registered for this trip");
            }

            if (trip.Registrations.Count() >= trip.NumberOfSeats)
            {
                throw new NoVacanciesForTripException("There is no vacancies for this trip");
            }

            await _registrationRepository.AddAsync(registration);
        }
    }
}
