using System.ComponentModel.DataAnnotations;

namespace Reservations.API.DTO;


public class BookDto
{
  [Required(ErrorMessage = "Book Title is required")]
  public int Id { get; set; }
  [Required(ErrorMessage = "Book Title is required")]
  [MinLength(2)]
  public string Title { get; set; } = String.Empty;
  public string? Author { get; set; }
  public bool IsReserved { get; set; }
  public string? ReservationComment { get; set; }
}


public class CreateBookDto
{
  [Required(ErrorMessage = "Book Title is required")]
  [MinLength(2)]
  public string Title { get; set; } = String.Empty;
  public string? Author { get; set; }
}


public class UpdateBookDto
{
  [Required(ErrorMessage = "Book Title is required")]
  [MinLength(2)]
  public string Title { get; set; } = String.Empty;
  public string? Author { get; set; }
}

