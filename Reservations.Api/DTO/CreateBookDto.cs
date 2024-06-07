namespace Reservations.API.DTO;

public class CreateBookDto
{
    [Required(ErrorMessage = "Book Title is required")]
    [MinLength(2)]
    public string Title { get; set; } = string.Empty;

    public string? Author { get; init; }
}