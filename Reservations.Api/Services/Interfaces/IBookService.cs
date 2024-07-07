namespace Reservations.Api.Services.Interfaces;

public interface IBookService : IBaseService<BookDto, CreateBookDto, UpdateBookDto>
{
    Task<BookDto?> ReserveBookAsync(int bookId, string comment);
    Task<bool> RemoveReservationAsync(int bookId);
    Task<IList<BookDto>> GetReservedBooksAsync();
    Task<IList<BookDto>> GetAvailableBooksAsync();
    Task<IList<ReservationHistoryDto>> GetSingleBookHistoryAsync(int bookId);
}