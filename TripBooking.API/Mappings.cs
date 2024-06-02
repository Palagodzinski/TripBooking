using AutoMapper;
using TripBooking.API.Models;
using TripBooking.Domain.Entities;

namespace TripBooking.API
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<TripDto, Trip>();
            CreateMap<Trip, TripSummaryDto>();
            CreateMap<Trip, TripDto>();

            CreateMap<RegistrationDto, Registration>();
        }
    }
}
