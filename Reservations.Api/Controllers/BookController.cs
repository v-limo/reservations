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

        // if (book is null)
        //     return BadRequest();

        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<BookDto>> GetBooks()
    {
        return await bookService.GetAllAsync();
    }


    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookDto>> GetBook(int id)
    {
        var book = await bookService.GetByIdAsync(id);
        return book;
    }


    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookDto>> UpdateBook(int id, UpdateBookDto updateBookDto)
    {
        if (id != updateBookDto.Id || !ModelState.IsValid)
            return BadRequest(
                new
                {
                    Message = "Book Id mismatch or invalid data",
                    BookId = id,
                    status = StatusCodes.Status400BadRequest,
                }
            );

        var book = await bookService.UpdateAsync(id, updateBookDto);
        if (book is null)
            return NotFound(
                new { Message = "Book not found so cannot update", BookId = id }
            );
        return book;
    }


    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> DeleteBook(int id)
    {
        var result = await bookService.DeleteAsync(id);
        if (!result)
            return NotFound(
                new { Message = "Book not found so cannot delete", BookId = id }
            );
        // return result; ////alternativly
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
    public async Task<IEnumerable<BookDto>> GetReservedBooks()
    {
        return await bookService.GetReservedBooksAsync();
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
    public async Task<IEnumerable<BookDto>> GetAvailableBooks()
    {
        return await bookService.GetAvailableBooksAsync();
    }


    [HttpGet("{bookId:int}/history")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<ReservationHistoryDto>> GetSingleBookHistoroy(int bookId)
    {
        var book = await bookService.GetByIdAsync(bookId);
        // TODO: what to rerturn if book is null ??

        return await bookService.getSingleBookHistoryAsync(bookId);
    }
}