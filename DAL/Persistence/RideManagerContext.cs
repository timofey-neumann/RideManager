using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Persistence;

public class RideManagerContext(DbContextOptions<RideManagerContext> options) : DbContext(options)
{
    public DbSet<Report> Reports { get; set; }

    public DbSet<Trip> Trips { get; set; }
    public DbSet<ReportDate> ReportDates { get; set; }
    public DbSet<SystemSetting> SystemSettings { get; set; }

    public DbSet<TransportCoordinator> TransportCoordinators { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trip>()
            .Property(e => e.CostWithoutVAT)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Trip>()
            .Property(e => e.VAT)
            .HasPrecision(18, 2);

        modelBuilder.Entity<SystemSetting>()
            .Property(e => e.HoursForPersonalTripStatusChange)
            .HasDefaultValue(24);
    }
}

