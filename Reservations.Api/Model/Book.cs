using System.ComponentModel.DataAnnotations;

namespace Reservations.API.Model;
public class Book : BaseEntity
{
    [Required(ErrorMessage = "Book Title is required")]
    [MinLength(2)]
    public string Title { get; set; } = string.Empty;
    public string? Author { get; set; } = string.Empty;

    public bool IsReserved { get; set; } = false;
    public string? ReservationComment { get; set; } = string.Empty;
    public List<ReservationHistory> ReservationHistories { get; set; } = [];
}
