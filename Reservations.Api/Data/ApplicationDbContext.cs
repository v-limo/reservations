namespace Reservations.Api.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; init; }
    public DbSet<ReservationHistory> ReservationHistory { get; init; }
}