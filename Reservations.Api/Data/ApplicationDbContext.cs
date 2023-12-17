using Microsoft.EntityFrameworkCore.Storage;

namespace Reservations.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        // Database.EnsureCreated();
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<ReservationHistory> ReservationHistory { get; set; }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    // }

    // public IDbContextTransaction BeginTransaction()
    // {
    //     return Database.BeginTransaction();
    // }
}
