using Microsoft.AspNetCore.Mvc;
using Reservations.Api.Services.Implementation;
using Reservations.Api.Services.Interfaces;
using Reservations.API.DTO;

namespace Reservations.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]s")]

[Produces("application/json")]
[Consumes("application/json")]

public class BookController(IBookService bookService) : ControllerBase
{
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
    if (book is null)
      return NotFound("Book not foundd");
    return book;
  }


  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<BookDto>> CreateBook(BookDto bookDto)
  {
    var book = await bookService.CreateAsync(bookDto);
    return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
  }


  [HttpPut("{id:int}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<BookDto>> UpdateBook(int id, BookDto bookDto)
  {
    var book = await bookService.UpdateAsync(id, bookDto);
    if (book is null)
      return NotFound("Book not found so cannot update");
    return book;
  }


  [HttpDelete("{id:int}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<bool>> DeleteBook(int id)
  {
    var result = await bookService.DeleteAsync(id);
    if (!result)
      return NotFound("Book not found so cannot delete");
    // return result; ////alternativly
    return NoContent();
  }


  [HttpPut("{bookId:int}/reserve")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<BookDto>> ReserveBook(int bookId, [FromBody] string comment)
  {
    var book = await bookService.ReserveBookAsync(bookId, comment);
    if (book is null)
      return NotFound("Book not found (or already reserved) so cannot reserve");
    return book;
  }


  [HttpPost("{bookId:int}/remove-reservation")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]

  public async Task<ActionResult<bool>> RemoveReservation(int bookId)
  {
    var result = await bookService.RemoveReservationAsync(bookId);
    if (!result)
      return NotFound("Book not found (or not reserved) so cannot remove reservation");
    return result;
  }


  [HttpGet("reserved-books")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public async Task<IEnumerable<BookDto>> GetReservedBooks()
  {
    return await bookService.GetReservedBooksAsync();
  }



  [HttpGet("available-books")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public async Task<IEnumerable<BookDto>> GetAvailableBooks()
  {
    return await bookService.GetAvailableBooksAsync();
  }
}
