using Reservations.API.DTO;

namespace Reservations.Api.Services.Interfaces;

public interface IBookService : IBaseService<BookDto>
{
    Task<BookDto?> ReserveBookAsync(int bookId, string comment);
    Task<bool> RemoveReservationAsync(int bookId);
    Task<IEnumerable<BookDto>> GetReservedBooksAsync();
    Task<IEnumerable<BookDto>> GetAvailableBooksAsync();
}
