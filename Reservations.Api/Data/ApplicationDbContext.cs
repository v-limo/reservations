using Microsoft.EntityFrameworkCore;
using Reservations.API.Model;

namespace Reservations.Api.Data;

public class ApplicationDbContext : DbContext
{
  public DbSet<Book> Books { get; set; }

  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
  {
  }
}
