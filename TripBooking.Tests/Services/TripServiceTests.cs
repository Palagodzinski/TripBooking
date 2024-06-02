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
    public class TripServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ITripRepository> _tripRepositoryMock;
        private readonly ITripService _tripService;

        public TripServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _tripRepositoryMock = _fixture.Freeze<Mock<ITripRepository>>();
            _tripService = _fixture.Create<TripService>();

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GivenTrip_WhenCreateTripAsync_ThenShouldReturnTripId()
        {
            // Arrange
            var trip = _fixture.Create<Trip>();
            _tripRepositoryMock.Setup(repo => repo.GetByNameAsync(trip.Name))
                .ReturnsAsync((Trip)null);
            _tripRepositoryMock.Setup(repo => repo.AddAsync(trip))
                .ReturnsAsync(trip.Id);

            // Act
            var result = await _tripService.CreateTripAsync(trip);

            // Assert
            result.Should().Be(trip.Id);
            _tripRepositoryMock.Verify(repo => repo.AddAsync(trip), Times.Once);
        }

        [Fact]
        public async Task GivenTripWithExistingName_WhenCreateTripAsync_ShouldThrowTripNameAlreadyExistsException()
        {
            // Arrange
            var trip = _fixture.Create<Trip>();
            _tripRepositoryMock.Setup(repo => repo.GetByNameAsync(trip.Name))
                .ReturnsAsync(trip);

            // Act
            Func<Task> act = async () => await _tripService.CreateTripAsync(trip);

            // Assert
            await act.Should().ThrowAsync<TripNameAlreadyExistsException>()
                .WithMessage($"Given trip name = {trip.Name} already exists");
        }

        [Fact]
        public async Task GivenTripWithNoExistingId_WhenUpdateTripAsync_ThenShouldThrowTripNotFoundException()
        {
            // Arrange
            var tripForUpdate = _fixture.Create<Trip>();
            _tripRepositoryMock.Setup(repo => repo.GetByIdAsync(tripForUpdate.Id))
                .ReturnsAsync((Trip)null);

            // Act
            Func<Task> act = async () => await _tripService.UpdateTripAsync(tripForUpdate.Id, tripForUpdate);

            // Assert
            await act.Should().ThrowAsync<TripNotFoundException>()
                .WithMessage($"There is no existing trip with given id = {tripForUpdate.Id}");
        }

        [Fact]
        public async Task GivenExistingTrip_WhenUpdateTripAsync_ThenShouldUpdateTrip()
        {
            // Arrange
            var tripId = _fixture.Create<int>();
            var existingTrip = _fixture.Create<Trip>();
            var tripForUpdate = _fixture.Create<Trip>();

            _tripRepositoryMock.Setup(repo => repo.GetByIdAsync(tripId))
                               .ReturnsAsync(existingTrip);
            _tripRepositoryMock.Setup(repo => repo.UpdateAsync(tripForUpdate, existingTrip))
                               .Returns(Task.CompletedTask);

            // Act
            Func<Task> act = async () => await _tripService.UpdateTripAsync(tripId, tripForUpdate);

            // Assert
            await act.Should().NotThrowAsync();

            _tripRepositoryMock.Verify(repo => repo.GetByIdAsync(tripId), Times.Once);
            _tripRepositoryMock.Verify(repo => repo.UpdateAsync(tripForUpdate, existingTrip), Times.Once);
        }

        [Fact]
        public async Task GivenTripWithNoExistingId_WhenDeleteTripAsync_ThenShouldThrowTripNotFoundException()
        {
            // Arrange
            var tripId = _fixture.Create<int>();
            _tripRepositoryMock.Setup(repo => repo.GetByIdAsync(tripId))
                .ReturnsAsync((Trip)null);

            // Act
            Func<Task> act = async () => await _tripService.DeleteTripAsync(tripId);

            // Assert
            await act.Should().ThrowAsync<TripNotFoundException>()
                .WithMessage($"There is no existing trip with given id = {tripId}");
        }

        [Fact]
        public async Task GivenTrip_WhenDeleteTripAsync_ThenShouldCallDeleteAsync()
        {
            // Arrange
            var trip = _fixture.Create<Trip>();
            _tripRepositoryMock.Setup(repo => repo.GetByIdAsync(trip.Id))
                .ReturnsAsync(trip);

            // Act
            await _tripService.DeleteTripAsync(trip.Id);

            // Assert
            _tripRepositoryMock.Verify(repo => repo.DeleteAsync(trip), Times.Once);
        }

        [Fact]
        public async Task GivenTrip_WhenGetTripByIdAsync_ThenShouldReturnTrip()
        {
            // Arrange
            var trip = _fixture.Create<Trip>();
            _tripRepositoryMock.Setup(repo => repo.GetByIdAsync(trip.Id))
                .ReturnsAsync(trip);

            // Act
            var result = await _tripService.GetTripByIdAsync(trip.Id);

            // Assert
            result.Should().Be(trip);
        }

        [Fact]
        public async Task GivenNotExistingTripId_WhenGetTripByIdAsync_ThenShouldReturnNull()
        {
            // Arrange
            var tripId = _fixture.Create<int>();
            _tripRepositoryMock.Setup(repo => repo.GetByIdAsync(tripId))
                .ReturnsAsync((Trip)null);

            // Act
            var result = await _tripService.GetTripByIdAsync(tripId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task WhenGetFirstOrDefaultTripAsync_ShouldReturnTrip()
        {
            // Arrange
            var trip = _fixture.Create<Trip>();
            _tripRepositoryMock.Setup(repo => repo.GetFirstOrDefaultAsync())
                .ReturnsAsync(trip);

            // Act
            var result = await _tripService.GetFirstOrDefaultTripAsync();

            // Assert
            result.Should().Be(trip);
        }

        [Fact]
        public async Task GivenNoExistingTrip_WhenGetFirstOrDefaultTripAsync_ThenShouldReturnNull()
        {
            // Arrange
            _tripRepositoryMock.Setup(repo => repo.GetFirstOrDefaultAsync())
                .ReturnsAsync((Trip)null);

            // Act
            var result = await _tripService.GetFirstOrDefaultTripAsync();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GivenCountryName_WhenGetTripsByCountryAsync_ShouldReturnTripsWithGivenCountry()
        {
            // Arrange
            var trips = _fixture.CreateMany<Trip>().ToList();
            var country = _fixture.Create<string>();
            _tripRepositoryMock.Setup(repo => repo.GetByCountryAsync(country))
                .ReturnsAsync(trips);

            // Act
            var result = await _tripService.GetTripsByCountryAsync(country);

            // Assert
            result.Should().BeEquivalentTo(trips);
        }

        [Fact]
        public async Task GivenIncorrectCountry_WhenGetTripsByCountryAsync_ShouldReturnEmpty()
        {
            // Arrange
            var country = _fixture.Create<string>();
            _tripRepositoryMock.Setup(repo => repo.GetByCountryAsync(country))
                .ReturnsAsync(new List<Trip>());

            // Act
            var result = await _tripService.GetTripsByCountryAsync(country);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task WhenGetAllTripsAsync_ThenShouldReturnAllTrips()
        {
            // Arrange
            var trips = _fixture.CreateMany<Trip>().ToList();
            _tripRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(trips);

            // Act
            var result = await _tripService.GetAllTripsAsync();

            // Assert
            result.Should().BeEquivalentTo(trips);
        }
    }
}
