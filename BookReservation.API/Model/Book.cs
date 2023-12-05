namespace Model;

public class Book : BaseEntity
{
  public string Title { get; set; }
  public string Author { get; set; }
  public bool IsReserved { get; set; }
  public string ReservationComment { get; set; }
}
