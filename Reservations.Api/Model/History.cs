
namespace Reservations.API.Model;
public class History
{
  public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
  public string? ReservationComment { get; set; }
  public int BookId { get; set; }
}
