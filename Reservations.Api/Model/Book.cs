namespace Reservations.API.Model;

public class Book : BaseEntity
{
    [Required(ErrorMessage = "Book Title is required")]
    [MinLength(2)]
    public required string Title { get; set; }

    public string? Author { get; set; } = null;

    public bool IsReserved { get; set; } = false;
    public string? ReservationComment { get; set; } = null;
    public List<ReservationHistory> ReservationHistories { get; set; } = new();
}