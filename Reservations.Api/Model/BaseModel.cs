using System.ComponentModel.DataAnnotations;

namespace Reservations.API.Model;

public class BaseEntity
{
  [Required(ErrorMessage = "Book Id is required")]
  public int Id { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
