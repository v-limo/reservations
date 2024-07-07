namespace Reservations.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]s")]
[Produces("application/json")]
[Consumes("application/json")]
public class BookController(IBookService bookService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookDto>> CreateBook(CreateBookDto bookDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var book = await bookService.CreateAsync(bookDto);

        // if (book is null) // TODO: what to do if book is null ?? GetBook throws exception
        //     return BadRequest();

        return CreatedAtAction(nameof(GetBook), new { bookId = book.Id }, book);
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IList<BookDto>> GetBooks()
    {
        return bookService.GetAllAsync();
    }


    [HttpGet("{bookId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookDto?>> GetBook(int bookId)
    {
        var book = await bookService.GetByIdAsync(bookId);
        if (book == null) return NotFound();

        return Ok(book);
    }

    [HttpPut("{bookId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookDto>> UpdateBook(int bookId, UpdateBookDto updateBookDto)
    {
        if (bookId != updateBookDto.Id || !ModelState.IsValid)
            return BadRequest(
                new
                {
                    Message = "Book Id mismatch or invalid data",
                    BookId = bookId,
                    status = StatusCodes.Status400BadRequest
                }
            );

        var book = await bookService.UpdateAsync(bookId, updateBookDto);
        if (book is null)
            return NotFound(
                new { Message = "Book not found so cannot update", BookId = bookId }
            );
        return book;
    }


    [HttpDelete("{bookId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> DeleteBook(int bookId)
    {
        var result = await bookService.DeleteAsync(bookId);
        if (!result)
            return NotFound(
                new { Message = "Book not found so cannot delete", BookId = bookId }
            );
        return NoContent();
    }


    [HttpPost("{bookId:int}/reserve/{*comment:maxlength(250)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookDto>> ReserveBook(int bookId, [FromRoute] string comment)
    {
        var book = await bookService.ReserveBookAsync(bookId, comment);
        if (book is null)
            return NotFound(
                new { Message = "Book not found (or already reserved) so cannot reserve", BookId = bookId }
            );
        return book;
    }


    [HttpGet("reserved-books")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IList<BookDto>> GetReservedBooks()
    {
        return bookService.GetReservedBooksAsync();
    }


    [HttpPost("{bookId:int}/remove-reservation")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> RemoveReservation(int bookId)
    {
        var result = await bookService.RemoveReservationAsync(bookId);
        if (!result)
            return NotFound(
                new
                {
                    Message = "Book not found (or was not reserved) so cannot remove reservation",
                    BookId = bookId
                }
            );
        return result;
    }


    [HttpGet("available-books")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IList<BookDto>> GetAvailableBooks()
    {
        return bookService.GetAvailableBooksAsync();
    }


    [HttpGet("{bookId:int}/history")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IList<ReservationHistoryDto>> GetSingleBookHistory(int bookId)
    {
        return bookService.GetSingleBookHistoryAsync(bookId);
    }
}