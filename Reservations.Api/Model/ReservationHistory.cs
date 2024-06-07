namespace Reservations.API.Model;

public class ReservationHistory
{
    public int Id { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime EventDate { get; set; } = DateTime.UtcNow;
    public ReservationAction Event { get; set; } = ReservationAction.Add;
    public int BookId { get; set; } // Foreign Key
    public required Book? Book { get; set; }
}