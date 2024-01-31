namespace Reservations.API.DTO;

public class UpdateBookDto
{
    [Required(ErrorMessage = "Book Title is required and must match the path id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Book Title is required")]
    [MinLength(2)]
    public required string Title { get; set; }

    public string? Author { get; set; }
}