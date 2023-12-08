namespace Reservations.API.DTO;

public class ReservationHistoryDto
{

    public string Comment { get; set; } = string.Empty;
    public DateTime EventDate { get; set; } = DateTime.UtcNow;
    public ReservationAction Event { get; set; } = ReservationAction.Add;
}
