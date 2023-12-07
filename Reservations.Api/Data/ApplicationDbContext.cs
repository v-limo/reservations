using Microsoft.EntityFrameworkCore;
using Reservations.API.Model;

namespace Reservations.Api.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
  public DbSet<Book> Books { get; set; }
}
