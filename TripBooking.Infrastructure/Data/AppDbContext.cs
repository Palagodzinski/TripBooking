using Microsoft.EntityFrameworkCore;
using TripBooking.Domain.Entities;

namespace TripBooking.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Registration> Registrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trip>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(t => t.Country)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(t => t.StartDate)
                    .IsRequired();

                entity.Property(t => t.NumberOfSeats)
                    .IsRequired();

                entity.HasMany(t => t.Registrations)
                    .WithOne(r => r.Trip)
                    .HasForeignKey(r => r.TripId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Registration>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Email)
                    .IsRequired();                    

                entity.Property(r => r.TripId)
                    .IsRequired();
            });
        }
    }
}
