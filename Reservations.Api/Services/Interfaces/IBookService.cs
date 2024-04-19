namespace Reservations.Api.Services.Interfaces;

public interface IBookService : IBaseService<BookDto, CreateBookDto, UpdateBookDto>
{
    Task<BookDto?> ReserveBookAsync(int bookId, string comment);
    Task<bool> RemoveReservationAsync(int bookId);
    Task<IEnumerable<BookDto>> GetReservedBooksAsync();
    Task<IEnumerable<BookDto>> GetAvailableBooksAsync();
    Task<IEnumerable<ReservationHistoryDto>> GetSingleBookHistoryAsync(int bookId);
}