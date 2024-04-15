namespace Reservations.Api.Services.Implementation;

public class BookService(
    ApplicationDbContext dbContext,
    IMapperBase mapper,
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

    public async Task<BookDto> CreateAsync(CreateBookDto createDto)
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

    public async Task<BookDto?> UpdateAsync(int id, UpdateBookDto updateDto)
    {
        try
        {
            var existingBook = await dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (existingBook == null)
                return null;


            existingBook.UpdatedAt =
                DateTime.UtcNow; //manual time update, because we don't want to allow user to update this field
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


            // TODO: Add delete logic for related entities
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
            if (book == null || book.IsReserved)
                return null;

            book.IsReserved = true;
            book.ReservationComment = comment;

            var history = new ReservationHistory
            {
                Comment = comment,
                BookId = book.Id,
                Book = book
            };

            await dbContext.ReservationHistory.AddAsync(history);

            book.ReservationHistories.Add(history);
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
            if (book == null || !book.IsReserved)
                return false;

            book.IsReserved = false;
            book.ReservationComment = null;

            var history = new ReservationHistory
            {
                Comment = $"Reservation for ${book.Id} removed",
                BookId = book.Id,
                Book = book,
                Event = ReservationAction.Remove
            };

            await dbContext.ReservationHistory.AddAsync(history);

            book.ReservationHistories.Add(history);
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

    public async Task<IEnumerable<ReservationHistoryDto>> GetSingleBookHistoryAsync(int bookId)
    {
        try
        {
            var histories = await dbContext.ReservationHistory.Where(x => x.BookId == bookId).ToListAsync();
            return mapper.Map<IEnumerable<ReservationHistoryDto>>(histories);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting book histories");
            throw;
        }
    }
}