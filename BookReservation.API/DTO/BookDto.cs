using System.ComponentModel.DataAnnotations;

namespace BookReservation.API.DTO;


public class BookDto : BaseDto
{

  [Required(ErrorMessage = "Book Title is required")]
  [MinLength(2)]
  public string Title { get; set; } = String.Empty;
  public string? Author { get; set; }
  public bool IsReserved { get; set; }
  public string? ReservationComment { get; set; }
}
