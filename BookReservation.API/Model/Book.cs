using System.ComponentModel.DataAnnotations;

namespace BookReservation.API.Model;
public class Book : BaseEntity
{
  [Required(ErrorMessage = "Book Title is required")]
  [MinLength(2)]

  public string Title { get; set; } = string.Empty;
  public string? Author { get; set; }
  public bool IsReserved { get; set; }
  public string? ReservationComment { get; set; }
}
