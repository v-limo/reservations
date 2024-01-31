namespace Reservations.Test.UnitTests;

public class BookControllerTests
{
    private readonly Mock<IBookService> _mockBookService = new();

    // 1. CreateAsync
    [Fact]
    public async Task CreateBook_WithValidData_ReturnsCreatedBook()
    {
        // Arrange
        var createBook = GetCreateBookDto();
        var bookDto = GetBooksDto().First();
        _mockBookService.Setup(x => x.CreateAsync(createBook)).ReturnsAsync(bookDto);
        var controller = new BookController(_mockBookService.Object);

        // Act
        var result = await controller.CreateBook(createBook);

        // Assert
        var book = result.Value;
        book?.Id.Should().Be(bookDto.Id);
        book?.Title.Should().Be(bookDto.Title);
        book?.Author.Should().Be(bookDto.Author);
    }

    [Fact]
    public async Task CreateBook_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        var book = GetCreateBookDto();
        var controller = new BookController(_mockBookService.Object);
        controller.ModelState.AddModelError("Title", "Title is required");

        // Act
        var result = await controller.CreateBook(book);

        // Assert
        result.Should().NotBeNull();
        result?.Result?.Should().BeOfType<BadRequestObjectResult>();
    }

    // 2. GetAllAsync
    [Fact]
    public async Task GetAllBooks_WithNoBooks_ReturnsEmptyList()
    {
        // Arrange
        List<BookDto> books = [];
        _mockBookService.Setup(x => x.GetAllAsync()).ReturnsAsync(books);
        var controller = new BookController(_mockBookService.Object);

        // Act
        var result = await controller.GetBooks();

        // Assert
        var bookDtos = result as BookDto[] ?? result.ToArray();
        bookDtos.Length.Should().Be(0);
        bookDtos.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAllBooks_WithBooks_ReturnsAllBooks()
    {
        // Arrange
        var books = GetBooksDto();
        _mockBookService.Setup(x => x.GetAllAsync()).ReturnsAsync(books);
        var controller = new BookController(_mockBookService.Object);

        // Act
        var result = await controller.GetBooks();

        // Assert
        var bookDto = result as BookDto[] ?? result.ToArray();
        bookDto.Should().BeEquivalentTo(books);
        bookDto.Should().NotBeNull();
    }


    // 3. GetByIdAsync
    [Fact]
    public async Task GetBookById_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var controller = new BookController(_mockBookService.Object);
        const int invalidId = 10;
        BookDto? book = null;
        _mockBookService.Setup(x => x.GetByIdAsync(invalidId))!.ReturnsAsync(book);

        // Act
        var result = await controller.GetBook(invalidId);

        // Assert
        result?.Result?.Should().BeOfType<
            NotFoundObjectResult
        >();
        result?.Result?.As<NotFoundObjectResult>()?.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetBookById_WithValidId_ReturnsBook()
    {
        // Arrange
        var book = GetBooksDto().First();
        _mockBookService.Setup(x => x.GetByIdAsync(book.Id)).ReturnsAsync(book);
        var controller = new BookController(_mockBookService.Object);

        // Act
        var result = await controller.GetBook(book.Id);

        // Assert
        result?.Value?.Id.Should().Be(book.Id);
        result?.Value?.Title.Should().Be(book.Title);
        result.Should().NotBeNull();
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

        _mockBookService.Setup(x => x.UpdateAsync(book.Id, update)).ReturnsAsync((BookDto)null);
        var controller = new BookController(_mockBookService.Object);

        // Act
        var result = await controller.UpdateBook(book.Id, update);

        // Assert
        result?.Result?.Should().BeOfType<BadRequestObjectResult>();
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

        var controller = new BookController(_mockBookService.Object);

        // Act
        var result = await controller.UpdateBook(book.Id, update);

        // Assert
        result?.Value?.Id.Should().Be(book.Id);
        result?.Value?.Title.Should().Be(book.Title);
        result?.Value?.Author.Should().Be(book.Author);
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

        var controller = new BookController(_mockBookService.Object);
        controller.ModelState.AddModelError("Title", "Title is required");

        // Act
        var result = await controller.UpdateBook(book.Id, update);

        // Assert
        result?.Result?.Should().BeOfType<BadRequestObjectResult>();
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

        var controller = new BookController(_mockBookService.Object);

        // Act
        var result = await controller.UpdateBook(book.Id, update);

        // Assert
        result?.Value?.Id.Should().Be(book.Id);
        result?.Value?.Title.Should().Be(book.Title);
        result?.Value?.Author.Should().Be(book.Author);
    }


    // 5. DeleteAsync
    [Fact]
    public async Task DeleteBook_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var books = GetBooksDto();
        const int nonExistingId = 10;
        _mockBookService.Setup(x => x.DeleteAsync(nonExistingId)).ReturnsAsync(false);
        var controller = new BookController(_mockBookService.Object);

        // Act
        var result = await controller.DeleteBook(nonExistingId);

        // Assert
        result?.Result?.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task DeleteBook_WithValidId_ReturnsNoContent()
    {
        // Arrange
        const int existingId = 1;
        _mockBookService.Setup(x => x.DeleteAsync(existingId)).ReturnsAsync(true);
        var controller = new BookController(_mockBookService.Object);

        // Act
        var result = await controller.DeleteBook(existingId);

        // Assert
        result.As<NoContentResult>()
            ?.StatusCode.Should().Be(204);
    }


    // 6. ReserveAsync
    [Fact]
    public async Task ReserveBook_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var nullBookDto = (BookDto)null;
        const int invalidId = 10;
        const string comment = "Comment: reserving book";

        _mockBookService.Setup(x => x.ReserveBookAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(nullBookDto);

        var controller = new BookController(_mockBookService.Object);

        // Act
        var result = await controller.ReserveBook(invalidId, comment);

        // Assert
        result?.Result?.Should().BeOfType<NotFoundObjectResult>();
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
        var controller = new BookController(_mockBookService.Object);

        // Act
        var result = await controller.ReserveBook(book.Id, comment);

        // Assert
        result.Should().NotBeNull();
        result?.Value?.Title.Should().Be(book.Title);
        result?.Value?.Author.Should().Be(book.Author);
        result?.Value?.IsReserved.Should().BeTrue();
        result?.Value?.ReservationComment.Should().Be(comment);
    }


    //7. RemoveReservationAsync
    [Fact]
    public async Task RemoveReservation_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        const int invalidId = 10;

        _mockBookService.Setup(x => x.RemoveReservationAsync(It.IsAny<int>())).ReturnsAsync(false);
        var controller = new BookController(_mockBookService.Object);

        // Act
        var result = await controller.RemoveReservation(invalidId);

        // Assert
        result?.Result?.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task RemoveReservation_WithValidId_ReturnsTrue()
    {
        // Arrange
        _mockBookService.Setup(x => x.RemoveReservationAsync(It.IsAny<int>())).ReturnsAsync(true);
        var controller = new BookController(_mockBookService.Object);

        // Act
        var result = await controller.RemoveReservation(1);

        // Assert
        result?.Result?.Should().BeOfType<NoContentResult>();
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
        var bookDtos = result as BookDto[] ?? result.ToArray();
        bookDtos.Length.Should().Be(0);
        bookDtos.Should().NotBeNull();
    }


    [Fact]
    public async Task GetAllReservedBooks_WithBooks_ReturnsAllBooks()
    {
        // Arrange
        var books = GetBooksDto();
        var reservedBooks = books.Where(x => x.IsReserved);
        _mockBookService.Setup(x => x.GetReservedBooksAsync()).ReturnsAsync(reservedBooks);

        var controller = new BookController(_mockBookService.Object);

        // Act
        var result = await controller.GetReservedBooks();

        // Assert
        var bookDtos = result as BookDto[] ?? result.ToArray();

        bookDtos.Length.Should().Be(2);
        bookDtos.Should().NotBeNull();
    }


    // 9. GetAllAvailableAsync
    [Fact]
    public async Task GetAllAvailableBooks_WithNoBooks_ReturnsEmptyList()
    {
        // Arrange
        var books = GetBooksDto();
        books.Clear();
        _mockBookService.Setup(x => x.GetAvailableBooksAsync()).ReturnsAsync(books);

        var controller = new BookController(_mockBookService.Object);

        // Act
        var result = await controller.GetAvailableBooks();

        // Assert
        var bookDtos = result as BookDto[] ?? result.ToArray();

        bookDtos.Length.Should().Be(0);
        bookDtos.Should().NotBeNull();
    }


    [Fact]
    public async Task GetAllAvailableBooks_WithBooks_ReturnsAllBooks()
    {
        // Arrange
        var books = GetBooksDto();
        var availableBooks = books.Where(x => !x.IsReserved);
        IEnumerable<BookDto> expectation = availableBooks as BookDto[] ?? availableBooks.ToArray();
        _mockBookService.Setup(x => x.GetAvailableBooksAsync()).ReturnsAsync(expectation);

        var controller = new BookController(_mockBookService.Object);

        // Act
        var result = await controller.GetAvailableBooks();

        // Assert
        var booksDto = result as BookDto[] ?? result.ToArray();

        booksDto.Should().BeEquivalentTo(expectation);
        booksDto.Should().NotBeNull();
    }

    //10. GetHistoryAsync
    [Fact]
    public async Task GetBooksHistory_WithNoBooks_ReturnsEmptyList()
    {
        // Arrange
        const int bookId = 1;
        var history = GetReservationHistoryDtos();

        _mockBookService.Setup(x => x.getSingleBookHistoryAsync(bookId)).ReturnsAsync(history);

        var controller = new BookController(_mockBookService.Object);

        // Act
        var result = await controller.GetSingleBookHistoroy(bookId);

        // Assert
        var historyDtos = result as ReservationHistoryDto[] ?? result.ToArray();

        historyDtos.Length.Should().Be(2);
        historyDtos.Should().NotBeNull();
    }

    // 10.2 Get books history with books
    [Fact]
    public async Task GetBooksHistory_WithBooks_ReturnsAllBooks()
    {
        // Arrange
        const int bookId = 3;
        var history = GetReservationHistoryDtos();

        _mockBookService.Setup(x => x.getSingleBookHistoryAsync(bookId)).ReturnsAsync(history);

        var controller = new BookController(_mockBookService.Object);

        // Act
        var result = await controller.GetSingleBookHistoroy(bookId);

        // Assert
        var historyDtos = result as ReservationHistoryDto[] ?? result.ToArray();

        historyDtos.Should().BeEquivalentTo(history);
        historyDtos.Should().NotBeNull();
    }

    private static List<BookDto> GetBooksDto()
    {
        var books = new List<BookDto>
        {
            new()
            {
                Id = 1,
                Title = "Book 1",
                Author = "Author 1",
                IsReserved = true
            },
            new()
            {
                Id = 2,
                Title = "Book 2",
                Author = "Author 2",
                IsReserved = true,
                ReservationComment = "Comment: reserving book"
            },
            new()
            {
                Id = 3,
                Title = "Book 3",
                Author = "Author 3",
                IsReserved = false,
                ReservationComment = null
            }
        };

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