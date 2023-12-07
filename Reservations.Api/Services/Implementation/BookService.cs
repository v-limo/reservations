using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reservations.API.DTO;
using Reservations.API.Model;
using Reservations.Api.Data;
using Reservations.Api.Services.Interfaces;

namespace Reservations.Api.Services.Implementation;

public class BookService(ApplicationDbContext dbContext, IMapper mapper,
ILogger<BookService> logger

) : IBookService
{
  public async Task<IEnumerable<BookDto>> GetAllAsync()
  {
    try
    {
      var books = await dbContext.Books.ToListAsync();
      return mapper.Map<IEnumerable<BookDto>>(books);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error while getting all books");
      throw;
    }
  }

  public async Task<BookDto> GetByIdAsync(int id)
  {
    try
    {
      var book = await dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
      return mapper.Map<BookDto>(book);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error while getting book by id");
      throw;
    }
  }

  public async Task<BookDto> CreateAsync(BookDto createDto)
  {
    try
    {
      var book = mapper.Map<Book>(createDto);
      dbContext.Books.Add(book);
      await dbContext.SaveChangesAsync();
      return mapper.Map<BookDto>(book);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error while creating book");
      throw;
    }
  }

  public async Task<BookDto?> UpdateAsync(int id, BookDto updateDto)
  {
    try
    {
      var existingBook = await dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
      if (existingBook == null)
        return null;

      mapper.Map(updateDto, existingBook);
      dbContext.Entry(existingBook).State = EntityState.Modified;
      await dbContext.SaveChangesAsync();
      return mapper.Map<BookDto>(existingBook);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error while updating book");
      throw;
    }
  }

  public async Task<bool> DeleteAsync(int id)
  {
    try
    {
      var existingBook = await dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
      if (existingBook == null)
        return false;

      dbContext.Books.Remove(existingBook);
      await dbContext.SaveChangesAsync();
      return true;
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error while deleting book");
      throw;
    }
  }

  public async Task<BookDto?> ReserveBookAsync(int bookId, string comment)
  {
    try
    {
      var book = await dbContext.Books.FirstOrDefaultAsync(x => x.Id == bookId);
      if (book == null)
        return null;

      book.IsReserved = true;
      book.ReservationComment = comment;
      await dbContext.SaveChangesAsync();
      return mapper.Map<BookDto>(book);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error while reserving book");
      throw;
    }
  }

  public async Task<bool> RemoveReservationAsync(int bookId)
  {
    try
    {
      var book = await dbContext.Books.FirstOrDefaultAsync(x => x.Id == bookId);
      if (book == null)
        return false;

      book.IsReserved = false;
      book.ReservationComment = null;
      await dbContext.SaveChangesAsync();
      return true;
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error while removing reservation");
      throw;
    }
  }

  public async Task<IEnumerable<BookDto>> GetReservedBooksAsync()
  {
    try
    {
      var books = await dbContext.Books.Where(x => x.IsReserved).ToListAsync();
      return mapper.Map<IEnumerable<BookDto>>(books);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error while getting reserved books");
      throw;
    }
  }

  public async Task<IEnumerable<BookDto>> GetAvailableBooksAsync()
  {
    try
    {
      var availableBooks = await dbContext.Books.Where(x => !x.IsReserved).ToListAsync();
      return mapper.Map<IEnumerable<BookDto>>(availableBooks);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error while getting available books");
      throw;
    }
  }
}
