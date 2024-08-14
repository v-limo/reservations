namespace Reservations.Test.UnitTests;

/// <summary>
///     Unit test for BookController
/// </summary>
public class BookControllerTests
{
    private readonly BookController _bookController;
    private readonly Mock<IBookService> _mockBookService = new();

    public BookControllerTests()
    {
        _bookController = new BookController(_mockBookService.Object);
    }

    // 1. CreateAsync
    [Fact]
    public async Task CreateBook_WithValidData_ReturnsCreatedBook()
    {
        // Arrange
        var bookDto = GetBooksDto().First();
        _mockBookService.Setup(x => x.CreateAsync(It.IsAny<CreateBookDto>())).ReturnsAsync(bookDto);

        // Act
        var result = await _bookController.CreateBook(It.IsAny<CreateBookDto>());

        // Assert
        var objectResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        objectResult.StatusCode.Should().Be(Status201Created);
        _ = objectResult.Value.Should().BeOfType<BookDto>().Subject;
    }

    [Fact]
    public async Task CreateBook_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        var book = GetCreateBookDto();
        _bookController.ModelState.AddModelError("Title", "Title is required");

        // Act
        var result = await _bookController.CreateBook(book);

        // Assert
        var badRequestResult = result.Result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult?.StatusCode.Should().Be(Status400BadRequest);
    }

    // 2. GetAllAsync
    [Fact]
    public async Task GetAllBooks_WithBooks_ReturnsAllBooks()
    {
        // Arrange
        var books = GetBooksDto();
        _mockBookService.Setup(x => x.GetAllAsync()).ReturnsAsync(books);

        // Act
        var result = await _bookController.GetBooks();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnBooks = okResult.Value.Should().BeAssignableTo<IEnumerable<BookDto>>().Subject;
        returnBooks.Should().BeEquivalentTo(books, options => options.ComparingByMembers<BookDto>());
        okResult.StatusCode.Should().Be(Status200OK);
    }

    // 3. GetByIdAsync
    [Fact]
    public async Task GetBookById_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        const int invalidId = 10;
        BookDto? book = null;
        _mockBookService.Setup(x => x.GetByIdAsync(invalidId)).ReturnsAsync(book);

        // Act
        var result = await _bookController.GetBook(invalidId);

        // Assert
        result.Result?.Should().BeOfType<
            NotFoundResult
        >();
        result.Result?.As<NotFoundResult>()?.StatusCode.Should().Be(Status404NotFound);
    }
    
    [Fact]
    public async Task GetBookById_WithValidId_ReturnsBook()
    {
        // Arrange
        var book = GetBooksDto().First();
        _mockBookService.Setup(x => x.GetByIdAsync(book.Id)).ReturnsAsync(book);

        // Act
        var result = await _bookController.GetBook(book.Id);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    // 4. UpdateAsync
    [Fact]
    public async Task UpdateBook_WithInvalidId_ReturnsBadRequest()
    {
        // Arrange
        var book = GetBooksDto().First();
        var update = new UpdateBookDto
        {
            Id = 10,
            Title = "Book 1",
            Author = "Author 1"
        };

        _mockBookService.Setup(x => x.UpdateAsync(book.Id, update)).ReturnsAsync((BookDto)null!);

        // Act
        var result = await _bookController.UpdateBook(book.Id, update);

        // Assert
        result.Result?.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should<int>().Be(400);
    }
    
    [Fact]
    public async Task UpdateBook_WithValidId_ReturnsUpdatedBook()
    {
        // Arrange
        var book = GetBooksDto().First();
        book.Title = "Updated Title";
        book.Author = "Updated Author";

        var update = new UpdateBookDto
        {
            Title = book.Title,
            Id = book.Id,
            Author = book.Author
        };

        _mockBookService.Setup(x => x.UpdateAsync(book.Id, update)).ReturnsAsync(book);

        // Act
        var result = await _bookController.UpdateBook(book.Id, update);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
    }
    
    [Fact]
    public async Task UpdateBook_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        var book = GetBooksDto().First();
        var update = new UpdateBookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author
        };

        _bookController.ModelState.AddModelError("Title", "Title is required");

        // Act
        var result = await _bookController.UpdateBook(book.Id, update);

        // Assert
        result.Result?.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Fact]
    public async Task UpdateBook_WithValidData_ReturnsUpdatedBook()
    {
        // Arrange
        var book = GetBooksDto().First();
        book.Title = "Updated Title";
        book.Author = "Updated Author";

        var update = new UpdateBookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author
        };

        _mockBookService.Setup(x => x.UpdateAsync(book.Id, update)).ReturnsAsync(book);


        // Act
        var result = await _bookController.UpdateBook(book.Id, update);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
    }

    // 5. DeleteAsync
    [Fact]
    public async Task DeleteBook_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        GetBooksDto();
        const int nonExistingId = 10;
        _mockBookService.Setup(x => x.DeleteAsync(nonExistingId)).ReturnsAsync(false);

        // Act
        var result = await _bookController.DeleteBook(nonExistingId);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>().Which.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task DeleteBook_WithValidId_ReturnsNoContent()
    {
        // Arrange
        const int existingId = 1;
        _mockBookService.Setup(x => x.DeleteAsync(existingId)).ReturnsAsync(true);

        // Act
        var result = await _bookController.DeleteBook(existingId);

        // Assert
        result.As<NoContentResult>()
            ?.StatusCode.Should().Be(204);
    }

    // 6. ReserveAsync
    [Fact]
    public async Task ReserveBook_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var nullBookDto = (BookDto)null!;
        const int invalidId = 10;
        const string comment = "Comment: reserving book";

        _mockBookService.Setup(x => x.ReserveBookAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(nullBookDto);


        // Act
        var result = await _bookController.ReserveBook(invalidId, comment);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>().Which.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task ReserveBook_WithValidIdAndComment_ReturnsReserveBook()
    {
        // Arrange
        var book = GetBooksDto().First();

        const string comment = "Comment: reserving book";
        _mockBookService.Setup(x => x.ReserveBookAsync(book.Id, It.IsAny<string>())).ReturnsAsync(
            new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                IsReserved = true,
                ReservationComment = comment
            }
        );

        // Act
        var result = await _bookController.ReserveBook(book.Id, comment);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(Status200OK);
    }

    //7. RemoveReservationAsync
    [Fact]
    public async Task RemoveReservation_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        const int invalidId = 10;

        _mockBookService.Setup(x => x.RemoveReservationAsync(It.IsAny<int>())).ReturnsAsync(false);

        // Act
        var result = await _bookController.RemoveReservation(invalidId);

        // Assert
        result.Result?.Should().BeOfType<NotFoundObjectResult>().Which.StatusCode.Should<int>().Be(404);
    }
    
    [Fact]
    public async Task RemoveReservation_WithValidId_ReturnsTrue()
    {
        // Arrange
        _mockBookService.Setup(x => x.RemoveReservationAsync(It.IsAny<int>())).ReturnsAsync(true);

        // Act
        var result = await _bookController.RemoveReservation(1);

        // Assert
        result.Result?.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should<int>().Be(200);
    }

    // 8. GetAllReservedAsync
    [Fact]
    public async Task GetAllReservedBooks_WithNoBooks_ReturnsEmptyList()
    {
        // Arrange
        var books = GetBooksDto();
        books.Clear();
        _mockBookService.Setup(x => x.GetReservedBooksAsync()).ReturnsAsync(books);

        var controller = new BookController(_mockBookService.Object);

        // Act
        var result = await controller.GetReservedBooks();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedBooks = okResult.Value.Should().BeAssignableTo<List<BookDto>>().Subject;

        returnedBooks.Count.Should().Be(0);
        returnedBooks.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAllReservedBooks_WithBooks_ReturnsAllBooks()
    {
        // Arrange
        var books = GetBooksDto();
        var reservedBooks = books.Where(x => x.IsReserved).ToList();
        _mockBookService.Setup(x => x.GetReservedBooksAsync()).ReturnsAsync(reservedBooks);

        // Act
        var result = await _bookController.GetReservedBooks();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedBooks = okResult.Value.Should().BeAssignableTo<List<BookDto>>().Subject;
        returnedBooks.Should().NotBeNull();
    }

    // 9. GetAllAvailableAsync
    [Fact]
    public async Task GetAllAvailableBooks_WithNoBooks_ReturnsEmptyList()
    {
        // Arrange
        _mockBookService.Setup(x => x.GetAvailableBooksAsync()).ReturnsAsync(new List<BookDto>());

        // Act
        var actionResult = await _bookController.GetAvailableBooks();

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should<int>().Be(Status200OK);
        okResult?.Value.Should().BeOfType<List<BookDto>>();
        (okResult?.Value as List<BookDto>)?.Count.Should().Be(0);
    }

    [Fact]
    public async Task GetAllAvailableBooks_WithBooks_ReturnsAllBooks()
    {
        // Arrange
        var books = GetBooksDto();
        var availableBooks = books.Where(x => !x.IsReserved).ToList();
        _mockBookService.Setup(x => x.GetAvailableBooksAsync()).ReturnsAsync(availableBooks);

        // Act
        var result = await _bookController.GetAvailableBooks();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedBooks = okResult.Value.Should().BeAssignableTo<List<BookDto>>().Subject;
        returnedBooks.Should().BeEquivalentTo(availableBooks);
        okResult.StatusCode.Should().Be(Status200OK);
    }
    
    //10. GetHistoryAsync
    [Fact]
    public async Task GetBooksHistory_WithNoBooks_ReturnsEmptyList()
    {
        // Arrange
        _mockBookService.Setup(x => x.GetSingleBookHistoryAsync(It.IsAny<int>())).ReturnsAsync([]);

        // Act
        var actionResult = await _bookController.GetSingleBookHistory(It.IsAny<int>());
        var result = actionResult.Result as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result?.StatusCode.Should().Be(200);
        result?.Value.Should().BeAssignableTo<IList<ReservationHistoryDto>>();
    }
    
    // 10.2 Get books history with books
    [Fact]
    public async Task GetBooksHistory_WithBooks_ReturnsAllBooks()
    {
        // Arrange
        var history = GetReservationHistoryDtos();

        _mockBookService.Setup(x => x.GetSingleBookHistoryAsync(It.IsAny<int>())).ReturnsAsync(history);

        // Act
        var actionResult = await _bookController.GetSingleBookHistory(It.IsAny<int>());
        var result = actionResult.Result as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result?.StatusCode.Should().Be(200);
    }
    
    private static List<BookDto> GetBooksDto()
    {
        List<BookDto> books =
        [
            new BookDto
            {
                Id = 1,
                Title = "Book 1",
                Author = "Author 1",
                IsReserved = true
            },

            new BookDto
            {
                Id = 2,
                Title = "Book 2",
                Author = "Author 2",
                IsReserved = true,
                ReservationComment = "Comment: reserving book"
            },
            new BookDto
            {
                Id = 3,
                Title = "Book 3",
                Author = "Author 3",
                IsReserved = false,
                ReservationComment = null
            }
        ];

        return books;
    }

    private static CreateBookDto GetCreateBookDto()
    {
        return new CreateBookDto
        {
            Title = "Book 1",
            Author = "Author 1"
        };
    }

    private static List<ReservationHistoryDto> GetReservationHistoryDtos()
    {
        var history = new List<ReservationHistoryDto>
        {
            new()
            {
                Id = 1,
                BookId = 1,
                Comment = "Comment: reserving book",
                Event = ReservationAction.Add,
                EventDate = DateTime.Now
            },
            new()
            {
                Id = 2,
                BookId = 1,
                Comment = "Comment: removing reservation",
                Event = ReservationAction.Remove,
                EventDate = DateTime.Now
            }
        };

        return history;
    }
}