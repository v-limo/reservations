using Microsoft.EntityFrameworkCore;
using Reservations.API.Model;

namespace Reservations.Api.Data;

class ApplicationDbContext : DbContext
{
  public DbSet<Book> Books { get; set; }

  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
  {
  }
}
