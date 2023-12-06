
namespace Reservations.API.Services;

public interface IBookService : IBaseService<Book, BookDto>
{
  Task<BookDto> ReserveBookAsync(int bookId, string comment);
  Task<bool> RemoveReservationAsync(int bookId);
  Task<IEnumerable<BookDto>> GetReservedBooksAsync();
  Task<IEnumerable<BookDto>> GetAvailableBooksAsync();
}
