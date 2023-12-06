using System.ComponentModel.DataAnnotations;

namespace Reservations.API.DTO;

public class BaseDto
{
  [Required]
  public int Id { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}
