using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TripBooking.API.Models;
using TripBooking.Application.Interfaces;
using TripBooking.Domain.Entities;

namespace TripBooking.API.Controllers
{
    [Route("api/trip")]
    [ApiController]
    public class TripController : Controller
    {
        private readonly ITripService _tripService;
        private readonly IRegistrationService _registrationService;
        private readonly IMapper _mapper;

        public TripController(
            ITripService tripService,
            IRegistrationService registrationService,
            IMapper mapper)
        {
            _tripService = tripService;
            _registrationService = registrationService;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates new trip
        /// </summary>
        /// <param name="tripDto"></param>
        /// <returns>Id of created trip</returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(TripDto tripDto)
        {
            var trip = _mapper.Map<Trip>(tripDto);
            var result = await _tripService.CreateTripAsync(trip);

            return Ok($"Id of created trip: {result}");
        }

        /// <summary>
        /// Updates existing trip
        /// </summary>
        /// <param name="id">Id of trip for update</param>
        /// <param name="tripDto">Changes for trip</param>        
        [HttpPut("{id}", Name = "Update")]
        public async Task<IActionResult> Update(int id, TripDto tripDto)
        {
            var trip = _mapper.Map<Trip>(tripDto);
            await _tripService.UpdateTripAsync(id, trip);

            return Ok();
        }

        /// <summary>
        /// Deletes existing trip
        /// </summary>
        /// <param name="id">Id of existing trip</param>        
        [HttpDelete("{id}", Name = "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _tripService.DeleteTripAsync(id);

            return Ok();
        }

        /// <summary>
        /// Gets all trips
        /// </summary>
        /// <returns>High-level information about all trips</returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var trips = await _tripService.GetAllTripsAsync();
            var tripsSummaries = _mapper.Map<IEnumerable<TripSummaryDto>>(trips);

            if (!tripsSummaries.Any())
            {
                return NotFound("There is no any trips");
            }

            return Ok(tripsSummaries);
        }

        /// <summary>
        /// Gets trips by country
        /// </summary>
        /// <param name="country">Name of country to search</param>
        /// <returns>High-level information about trips for given country</returns>
        [HttpGet("{country}", Name = "GetByCountry")]
        public async Task<IActionResult> GetByCountry(string country)
        {
            var trips = await _tripService.GetTripsByCountryAsync(country);
            var tripsSummaries = _mapper.Map<IEnumerable<TripSummaryDto>>(trips);

            if (!tripsSummaries.Any())
            {
                return NotFound($"There is no any trips with given country = {country}");
            }

            return Ok(tripsSummaries);
        }

        /// <summary>
        /// Gets single trip
        /// </summary>
        /// <returns>Details of single trip</returns>
        [HttpGet("GetFirstWithAllData")]
        public async Task<IActionResult> GetFirstWithAllData()
        {
            var trip = await _tripService.GetFirstOrDefaultTripAsync();
            var tripDto = _mapper.Map<TripDto>(trip);

            if (trip == null)
            {
                return NotFound("There is no any trips");
            }

            return Ok(tripDto);
        }

        /// <summary>
        /// Registers user email for trip
        /// </summary>
        /// <param name="registrationDto"></param>        
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegistrationDto registrationDto)
        {
            var registration = _mapper.Map<Registration>(registrationDto);
            await _registrationService.RegisterAsync(registration);

            return Ok();
        }
    }
}
