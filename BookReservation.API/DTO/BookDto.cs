namespace BookReservation.API.DTO;


public class BookDto : BaseDto
{

  public int Id { get; set; }
  public string Title { get; set; }
  public string Author { get; set; }
  public bool IsReserved { get; set; }
  public string ReservationComment { get; set; }
}
