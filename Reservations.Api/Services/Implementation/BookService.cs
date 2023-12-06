using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reservations.API.DTO;
using Reservations.API.Model;
using Reservations.Api.Data;
using Reservations.Api.Services.Interfaces;

namespace Reservations.Api.Services.Implementation;

public class BookService(ApplicationDbContext dbContext, IMapper mapper) : IBookService
{
  public async Task<IEnumerable<BookDto>> GetAllAsync()
  {
    var books = await dbContext.Books.ToListAsync();
    return mapper.Map<IEnumerable<BookDto>>(books);
  }

  public async Task<BookDto> GetByIdAsync(int id)
  {
    var book = await dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
    return mapper.Map<BookDto>(book);
  }

  public async Task<BookDto> CreateAsync(BookDto createDto)
  {
    var book = mapper.Map<Book>(createDto);
    dbContext.Books.Add(book);
    await dbContext.SaveChangesAsync();
    return mapper.Map<BookDto>(book);
  }

  public async Task<BookDto?> UpdateAsync(int id, BookDto updateDto)
  {
    var existingBook = await dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
    if (existingBook == null)
      return null;

    mapper.Map(updateDto, existingBook);
    dbContext.Entry(existingBook).State = EntityState.Modified;
    await dbContext.SaveChangesAsync();
    return mapper.Map<BookDto>(existingBook);
  }

  public async Task<bool> DeleteAsync(int id)
  {
    var existingBook = await dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
    if (existingBook == null)
      return false;

    dbContext.Books.Remove(existingBook);
    await dbContext.SaveChangesAsync();
    return true;
  }

  public async Task<BookDto?> ReserveBookAsync(int bookId, string comment)
  {
    var book = await dbContext.Books.FirstOrDefaultAsync(x => x.Id == bookId);
    if (book == null)
      return null;

    book.IsReserved = true;
    book.ReservationComment = comment;
    await dbContext.SaveChangesAsync();
    return mapper.Map<BookDto>(book);
  }

  public async Task<bool> RemoveReservationAsync(int bookId)
  {
    var book = await dbContext.Books.FirstOrDefaultAsync(x => x.Id == bookId);
    if (book == null)
      return false;

    book.IsReserved = false;
    book.ReservationComment = null;
    await dbContext.SaveChangesAsync();
    return true;
  }

  public async Task<IEnumerable<BookDto>> GetReservedBooksAsync()
  {
    var books = await dbContext.Books.Where(x => x.IsReserved).ToListAsync();
    return mapper.Map<IEnumerable<BookDto>>(books);
  }

  public async Task<IEnumerable<BookDto>> GetAvailableBooksAsync()
  {
    var availableBooks = await dbContext.Books.Where(x => !x.IsReserved).ToListAsync();
    return mapper.Map<IEnumerable<BookDto>>(availableBooks);
  }
}
