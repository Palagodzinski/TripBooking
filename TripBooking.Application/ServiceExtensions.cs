using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TripBooking.Application.Interfaces;
using TripBooking.Application.Services;
using TripBooking.Infrastructure.Data;
using TripBooking.Infrastructure.Interfaces;
using TripBooking.Infrastructure.Repositories;

namespace TripBooking.Application
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services) 
        {
            services.AddScoped<ITripService, TripService>();
            services.AddScoped<IRegistrationService, RegistrationService>();

            services.AddScoped<ITripRepository, TripRepository>();
            services.AddScoped<IRegistrationRepository, RegistrationRepository>();

            return services;
        }

        public static IServiceCollection AddInMemoryDb(this IServiceCollection services) =>
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TripBookingDb"));        
    }
}
