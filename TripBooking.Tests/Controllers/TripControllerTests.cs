using AutoFixture.AutoMoq;
using AutoFixture;
using Moq;
using TripBooking.Application.Interfaces;
using AutoMapper;
using TripBooking.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using TripBooking.API.Models;
using TripBooking.Domain.Entities;
using FluentAssertions;

namespace TripBooking.Tests.Controllers
{
    public class TripControllerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ITripService> _tripServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly TripController _tripController;
        private readonly Mock<IRegistrationService> _registrationServiceMock;

        private readonly string TripName = "TestTripName";
        private readonly string TripCountry = "TestTripCountry";
        private readonly DateTime TripStartDate = DateTime.Now;

        public TripControllerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _tripServiceMock = _fixture.Freeze<Mock<ITripService>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _registrationServiceMock = new Mock<IRegistrationService>();
            _tripController = new TripController(_tripServiceMock.Object, _registrationServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task WhenGetAll_ThenShouldReturnOkWithTripSummaries()
        {
            // Arrange

            var trips = new List<Trip> { new Trip { Name = TripName, Country = TripCountry, StartDate = TripStartDate } };
            var tripSummaries = new List<TripSummaryDto> { new TripSummaryDto(TripName, TripCountry, TripStartDate) };

            _tripServiceMock.Setup(service => service.GetAllTripsAsync()).ReturnsAsync(trips);
            _mapperMock.Setup(m => m.Map<IEnumerable<TripSummaryDto>>(trips)).Returns(tripSummaries);

            // Act
            var result = await _tripController.GetAll() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.Value.Should().BeEquivalentTo(tripSummaries);
        }

        [Fact]
        public async Task GivenCorrectCountry_WhenGetByCountry_ThenShouldReturnOkWithTripSummaries()
        {
            // Arrange
            var trips = new List<Trip> { new Trip { Name = TripName, Country = TripCountry } };
            var tripSummaries = new List<TripSummaryDto> { new TripSummaryDto(TripName, TripCountry, TripStartDate) };

            _tripServiceMock.Setup(service => service.GetTripsByCountryAsync(TripCountry)).ReturnsAsync(trips);
            _mapperMock.Setup(m => m.Map<IEnumerable<TripSummaryDto>>(trips)).Returns(tripSummaries);

            // Act
            var result = await _tripController.GetByCountry(TripCountry) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.Value.Should().BeEquivalentTo(tripSummaries);
        }

        [Fact]
        public async Task GetFirstWithAllData_ShouldReturnOkWithTrip()
        {
            // Arrange
            var trip = new Trip { Name = TripName };
            var tripDto = new TripDto { Name = TripName };

            _tripServiceMock.Setup(service => service.GetFirstOrDefaultTripAsync()).ReturnsAsync(trip);
            _mapperMock.Setup(m => m.Map<TripDto>(trip)).Returns(tripDto);

            // Act
            var result = await _tripController.GetFirstWithAllData() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.Value.Should().BeEquivalentTo(tripDto);
        }
    }
}