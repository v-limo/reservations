namespace Reservations.API.DTO;

public class BookDto
{
    [Required(ErrorMessage = "Book Title is required")]
    public int Id { get; init; }

    [Required(ErrorMessage = "Book Title is required")]
    [MinLength(2)]
    public string Title { get; set; } = string.Empty;

    public string? Author { get; set; }
    public bool IsReserved { get; init; }
    public string? ReservationComment { get; init; }
}