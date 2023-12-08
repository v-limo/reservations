
namespace Reservations.API.Model;
public class ReservationHistory
{
  public int Id { get; set; }
  public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
  public string ReservationComment { get; set; } = string.Empty;
  public DateTime? ReturnDate { get; set; }

}
