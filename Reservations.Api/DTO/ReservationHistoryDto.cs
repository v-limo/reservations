namespace Reservations.API.DTO;

public class ReservationHistoryDto
{
  public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
  public DateTime? ReturnDate { get; set; }
  public string? ReservationComment { get; set; }
}
