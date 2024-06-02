using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Moq;
using TripBooking.Application.Exceptions;
using TripBooking.Application.Interfaces;
using TripBooking.Application.Services;
using TripBooking.Domain.Entities;
using TripBooking.Infrastructure.Interfaces;

namespace TripBooking.Tests.Services
{
    public class RegistrationServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IRegistrationRepository> _registrationRepositoryMock;
        private readonly Mock<ITripService> _tripServiceMock;
        private readonly RegistrationService _registrationService;

        public RegistrationServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _registrationRepositoryMock = _fixture.Freeze<Mock<IRegistrationRepository>>();
            _tripServiceMock = _fixture.Freeze<Mock<ITripService>>();
            _registrationService = _fixture.Create<RegistrationService>();

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GivenRegistrationWithInvalidTripId_WhenRegisterAsync_ThenShouldThrowTripNotFoundException()
        {
            // Arrange
            var registration = _fixture.Create<Registration>();
            _tripServiceMock.Setup(service => service.GetTripByIdAsync(registration.TripId))
                .ReturnsAsync((Trip)null);

            // Act
            Func<Task> act = async () => await _registrationService.RegisterAsync(registration);

            // Assert
            await act.Should().ThrowAsync<TripNotFoundException>()
                .WithMessage($"There is no existing trip with given id = {registration.TripId}");
        }

        [Fact]
        public async Task GivenExistingRegistrationEmail_WhenRegisterAsync_ThenShouldThrowEmailAlreadyRegisteredForTripException()
        {
            // Arrange
            var registration = _fixture.Create<Registration>();
            var trip = _fixture.Create<Trip>();
            _tripServiceMock.Setup(service => service.GetTripByIdAsync(registration.TripId))
                .ReturnsAsync(trip);
            _registrationRepositoryMock.Setup(repo => repo.IsEmailRegisteredForTripAsync(registration.Email, registration.TripId))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _registrationService.RegisterAsync(registration);

            // Assert
            await act.Should().ThrowAsync<EmailAlreadyRegisteredForTripException>()
                .WithMessage($"Email = {registration.Email} is already registered for this trip");
        }

        [Fact]
        public async Task GivenRegistrationForFullTrip_WhenRegisterAsync_ThenShouldThrowNoVacanciesForTripException()
        {
            // Arrange
            var registration = _fixture.Create<Registration>();
            var trip = _fixture.Build<Trip>()
                .With(t => t.NumberOfSeats, 1)
                .With(t => t.Registrations, new List<Registration> { new Registration { Email = "test@example.com" } })
                .Create();

            _tripServiceMock.Setup(service => service.GetTripByIdAsync(registration.TripId))
                .ReturnsAsync(trip);
            _registrationRepositoryMock.Setup(repo => repo.IsEmailRegisteredForTripAsync(registration.Email, registration.TripId))
                .ReturnsAsync(false);

            // Act
            Func<Task> act = async () => await _registrationService.RegisterAsync(registration);

            // Assert
            await act.Should().ThrowAsync<NoVacanciesForTripException>()
                .WithMessage("There is no vacancies for this trip");
        }

        [Fact]
        public async Task GivenValidRegistration_WhenRegisterAsync_ThenShouldCallAddAsync()
        {
            // Arrange
            var registration = _fixture.Create<Registration>();
            var trip = _fixture.Build<Trip>()
                .With(t => t.NumberOfSeats, 10)
                .With(t => t.Registrations, new List<Registration>())
                .Create();
            _tripServiceMock.Setup(service => service.GetTripByIdAsync(registration.TripId))
                .ReturnsAsync(trip);
            _registrationRepositoryMock.Setup(repo => repo.IsEmailRegisteredForTripAsync(registration.Email, registration.TripId))
                .ReturnsAsync(false);

            // Act
            await _registrationService.RegisterAsync(registration);

            // Assert
            _registrationRepositoryMock.Verify(repo => repo.AddAsync(registration), Times.Once);
        }
    }
}
