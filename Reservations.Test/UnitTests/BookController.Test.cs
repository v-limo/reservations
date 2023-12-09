namespace Reservations.Test.UnitTests;

public class BookControllerTests
{
    private readonly Mock<IBookService> _mockBookService = new();

    // 1. Add a new book - CreateAsync
    // 1.2 Add a new book with valid data
    [Fact]
    public async Task CreateBook_WithValidData_ReturnsCreatedBook()
    {
        // Arrange
        var createBook = GetreateBookDto();
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

    // 1.1 Add a new book with invalid data
    [Fact]
    public async Task CreateBook_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        var book = GetreateBookDto();
        var controller = new BookController(_mockBookService.Object);
        controller.ModelState.AddModelError("Title", "Title is required");

        // Act
        var result = await controller.CreateBook(book);

        // Assert
        result.Should().NotBeNull();
        result?.Result?.Should().BeOfType<BadRequestObjectResult>();
    }

    // 2. Get all books - GetAllAsync
    // 2.1 Get all books with no books
    [Fact]
    public async Task GetAllBooks_WithNoBooks_ReturnsEmptyList()
    {
        // Arrange
        var books = new List<BookDto>();
        _mockBookService.Setup(x => x.GetAllAsync()).ReturnsAsync(books);
        var controller = new Api.Controllers.BookController(_mockBookService.Object);

        // Act
        var result = await controller.GetBooks();

        // Assert
        var bookDtos = result as BookDto[] ?? result.ToArray();
        bookDtos.Count().Should().Be(0);
        bookDtos.Should().NotBeNull();
    }
    // 2.2 Get all books with books

    [Fact]
    public async Task GetAllBooks_WithBooks_ReturnsAllBooks()
    {
        // Arrange
        var books = GetBooksDto();
        _mockBookService.Setup(x => x.GetAllAsync()).ReturnsAsync(books);
        var controller = new Api.Controllers.BookController(_mockBookService.Object);

        // Act
        var result = await controller.GetBooks();

        // Assert
        var bookDtos = result as BookDto[] ?? result.ToArray();
        bookDtos.Should().BeEquivalentTo(books);
        bookDtos.Should().NotBeNull();
    }


    // 3. Get book by id - GetByIdAsync
    // 3.1 Get book by id with invalid id

    [Fact]
    public async Task GetBookById_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var controller = new Api.Controllers.BookController(_mockBookService.Object);
        var invalidId = 10;
        var book = GetBooksDto().First();
        book = null;

        _mockBookService.Setup(x => x.GetByIdAsync(invalidId)).ReturnsAsync(book);

        // Act
        var result = await controller.GetBook(invalidId);

        // Assert
        result?.Result?.Should().BeOfType<
            NotFoundObjectResult
        >();
        result?.Result?.As<NotFoundObjectResult>()?.StatusCode.Should().Be(404);
    }

    // 3.2 Get book by id with valid id
    [Fact]
    public async Task GetBookById_WithValidId_ReturnsBook()
    {
        // Arrange
        var book = GetBooksDto().First();
        _mockBookService.Setup(x => x.GetByIdAsync(book.Id)).ReturnsAsync(book);
        var controller = new Api.Controllers.BookController(_mockBookService.Object);

        // Act
        var result = await controller.GetBook(book.Id);

        // Assert
        var bookDto = result;
        bookDto?.Value?.Id.Should().Be(book.Id);
        bookDto?.Value?.Title.Should().Be(book.Title);
        bookDto.Should().NotBeNull();
    }


    // 4. Update book - UpdateAsync
    // 4.1 Update book with invalid id
    [Fact]
    public async Task UpdateBook_WithInvalidId_ReturnsBadRequest()
    {
        // Arrange
        var book = GetBooksDto().First();

        var update = new UpdateBookDto
        {
            Id = 10,
            Title = "Book 1",
            Author = "Author 1",
        };

        _mockBookService.Setup(x => x.UpdateAsync(book.Id, update)).ReturnsAsync((BookDto)null);
        var controller = new Api.Controllers.BookController(_mockBookService.Object);

        // Act
        var result = await controller.UpdateBook(book.Id, update);

        // Assert
        result?.Result?.Should().BeOfType<BadRequestObjectResult>();
    }

    // 4.2 Update book with valid id
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
        };
        update.Id = book.Id;
        update.Author = book.Author;

        _mockBookService.Setup(x => x.UpdateAsync(book.Id, update)).ReturnsAsync(book);

        var controller = new Api.Controllers.BookController(_mockBookService.Object);

        // Act
        var result = await controller.UpdateBook(book.Id, update);

        // Assert
        var bookDto = result;
        bookDto?.Value?.Id.Should().Be(book.Id);
        bookDto?.Value?.Title.Should().Be(book.Title);
        bookDto?.Value?.Author.Should().Be(book.Author);
    }

    // 4.3 Update book with invalid data
    [Fact]
    public async Task UpdateBook_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        var book = GetBooksDto().First();
        var update = new UpdateBookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
        };

        var controller = new Api.Controllers.BookController(_mockBookService.Object);
        controller.ModelState.AddModelError("Title", "Title is required");

        // Act
        var result = await controller.UpdateBook(book.Id, update);

        // Assert
        result?.Result?.Should().BeOfType<BadRequestObjectResult>();
    }


    // 4.4 Update book with valid data
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
            Author = book.Author,
        };

        _mockBookService.Setup(x => x.UpdateAsync(book.Id, update)).ReturnsAsync(book);

        var controller = new Api.Controllers.BookController(_mockBookService.Object);

        // Act
        var result = await controller.UpdateBook(book.Id, update);

        // Assert
        var bookDto = result;
        bookDto?.Value?.Id.Should().Be(book.Id);
        bookDto?.Value?.Title.Should().Be(book.Title);
        bookDto?.Value?.Author.Should().Be(book.Author);
    }


    // 5. Delete book - DeleteAsync
    // 5.1 Delete book with invalid id
    [Fact]
    public async Task DeleteBook_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var books = GetBooksDto();
        var nonExistingId = 10;
        _mockBookService.Setup(x => x.DeleteAsync(nonExistingId)).ReturnsAsync(false);

        var controller = new Api.Controllers.BookController(_mockBookService.Object);

        // Act

        var result = await controller.DeleteBook(nonExistingId);
        // Assert
        result?.Result?.Should().BeOfType<NotFoundObjectResult>();
    }

    // 5.2 Delete book with valid id
    [Fact]
    public async Task DeleteBook_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var books = GetBooksDto();
        var existingId = 1;
        _mockBookService.Setup(x => x.DeleteAsync(existingId)).ReturnsAsync(true);
        var controller = new Api.Controllers.BookController(_mockBookService.Object);

        // Act
        var result = await controller.DeleteBook(existingId);

        // Assert
        result.As<NoContentResult>()
            ?.StatusCode.Should().Be(204);
    }


    // 6. Reserve a book - ReserveAsync

    // 6.1 Reserve a book with invalid id and comment
    [Fact]
    public async Task ReserveBook_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var nullBookDto = (BookDto)null;
        var invalidId = 10;
        var comment = "Comment: reserving book";

        _mockBookService.Setup(x => x.ReserveBookAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(nullBookDto);

        var controller = new Api.Controllers.BookController(_mockBookService.Object);

        // Act
        var result = await controller.ReserveBook(invalidId, comment);

        // Assert
        result?.Result?.Should().BeOfType<NotFoundObjectResult>();
    }

    // 6.2 Reserve a book with valid id and comment
    [Fact]
    public async Task ReserveBook_WithValidIdAndComment_ReturnsReserveBook()
    {
        // Arrange
        var book = GetBooksDto().First();
        // book.IsReserved = true;
        var comment = "Comment: reserving book";
        _mockBookService.Setup(x => x.ReserveBookAsync(book.Id, It.IsAny<string>())).ReturnsAsync(
            new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                IsReserved = true,
                ReservationComment = comment,
            }
        );
        var controller = new Api.Controllers.BookController(_mockBookService.Object);

        // Act
        var result = await controller.ReserveBook(book.Id, comment);

        // Assert
        result.Should().NotBeNull();
        result?.Value?.Title.Should().Be(book.Title);
        result?.Value?.Author.Should().Be(book.Author);
        result?.Value?.IsReserved.Should().BeTrue();
        result?.Value?.ReservationComment.Should().Be(comment);
    }


    // 7. Remove reservation - RemoveReservationAsync

    // 7.1 Remove reservation with invalid id
    [Fact]
    public async Task RemoveReservation_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = 10;

        _mockBookService.Setup(x => x.RemoveReservationAsync(It.IsAny<int>())).ReturnsAsync(false);
        var controller = new Api.Controllers.BookController(_mockBookService.Object);

        // Act
        var result = await controller.RemoveReservation(invalidId);

        // Assert
        result?.Result?.Should().BeOfType<NotFoundObjectResult>();
    }

    // 7.2 Remove reservation with valid id
    [Fact]
    public async Task RemoveReservation_WithValidId_ReturnsTrue()
    {
        // Arrange
        _mockBookService.Setup(x => x.RemoveReservationAsync(It.IsAny<int>())).ReturnsAsync(true);
        var controller = new Api.Controllers.BookController(_mockBookService.Object);

        // Act
        var result = await controller.RemoveReservation(1);

        // Assert
        result?.Result?.Should().BeOfType<NoContentResult>();
    }

    // 8. Get all reserved books - GetAllReservedAsync

    // 8.1 Get all reserved books with no books
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
        bookDtos.Count().Should().Be(0);
        bookDtos.Should().NotBeNull();
    }


    // 8.2 Get all reserved books with books
    [Fact]
    public async Task GetAllReservedBooks_WithBooks_ReturnsAllBooks()
    {
        // Arrange
        var books = GetBooksDto();
        var reservedBooks = books.Where(x => x.IsReserved);
        _mockBookService.Setup(x => x.GetReservedBooksAsync()).ReturnsAsync(reservedBooks);

        var controller = new Api.Controllers.BookController(_mockBookService.Object);

        // Act
        var result = await controller.GetReservedBooks();

        // Assert
        var bookDtos = result as BookDto[] ?? result.ToArray();

        bookDtos.Count().Should().Be(2);
        bookDtos.Should().NotBeNull();
    }


    // 9. Get available books - GetAllAvailableAsync

    // 9.1 Get available books with no books
    [Fact]
    public async Task GetAllAvailableBooks_WithNoBooks_ReturnsEmptyList()
    {
        // Arrange
        var books = GetBooksDto();
        books.Clear();
        _mockBookService.Setup(x => x.GetAvailableBooksAsync()).ReturnsAsync(books);

        var controller = new Api.Controllers.BookController(_mockBookService.Object);

        // Act
        var result = await controller.GetAvailableBooks();

        // Assert
        var bookDtos = result as BookDto[] ?? result.ToArray();

        bookDtos.Count().Should().Be(0);
        bookDtos.Should().NotBeNull();
    }

    // 9.2 Get available books with books
    [Fact]
    public async Task GetAllAvailableBooks_WithBooks_ReturnsAllBooks()
    {
        // Arrange
        var books = GetBooksDto();
        var availableBooks = books.Where(x => !x.IsReserved);
        _mockBookService.Setup(x => x.GetAvailableBooksAsync()).ReturnsAsync(availableBooks);

        var controller = new Api.Controllers.BookController(_mockBookService.Object);

        // Act
        var result = await controller.GetAvailableBooks();

        // Assert
        var bookDtos = result as BookDto[] ?? result.ToArray();

        bookDtos.Should().BeEquivalentTo(availableBooks);
        bookDtos.Should().NotBeNull();
    }


    // 10. Get books history - GetHistoryAsync
    // 10.1 Get books history with no books
    [Fact]
    public async Task GetBooksHistory_WithNoBooks_ReturnsEmptyList()
    {
        // Arrange
        var bookId = 1;
        var history = GetReservationHistoryDtos();

        _mockBookService.Setup(x => x.getSingleBookHistoryAsync(bookId)).ReturnsAsync(history);

        var controller = new Api.Controllers.BookController(_mockBookService.Object);

        // Act
        var result = await controller.GetSingleBookHistoroy(bookId);

        // Assert
        var historyDtos = result as ReservationHistoryDto[] ?? result.ToArray();

        historyDtos.Count().Should().Be(2);
        historyDtos.Should().NotBeNull();
    }

    // 10.2 Get books history with books
    [Fact]
    public async Task GetBooksHistory_WithBooks_ReturnsAllBooks()
    {
        // Arrange
        var bookId = 3;
        var history = GetReservationHistoryDtos();

        _mockBookService.Setup(x => x.getSingleBookHistoryAsync(bookId)).ReturnsAsync(history);

        var controller = new Api.Controllers.BookController(_mockBookService.Object);

        // Act
        var result = await controller.GetSingleBookHistoroy(bookId);

        // Assert
        var historyDtos = result as ReservationHistoryDto[] ?? result.ToArray();

        historyDtos.Should().BeEquivalentTo(history);
        historyDtos.Should().NotBeNull();
    }

    private List<Book> GetBooksData()
    {
        List<Book> books = new List<Book>
        {
            new Book
            {
                Id = 1,
                Title = "Book 1",
                Author = "Author 1",
            },
            new Book
            {
                Id = 2,
                Title = "Book 2",
                Author = "Author 2",
            },
            new Book
            {
                Id = 3,
                Title = "Book 3",
                Author = "Author 3",
            }
        };

        return books;
    }

    private List<BookDto> GetBooksDto()
    {
        List<BookDto> books = new List<BookDto>
        {
            new BookDto
            {
                Id = 1,
                Title = "Book 1",
                Author = "Author 1",
                IsReserved = true,
            },
            new BookDto
            {
                Id = 2,
                Title = "Book 2",
                Author = "Author 2",
                IsReserved = true,
                ReservationComment = "Comment: reserving book",
            },
            new BookDto
            {
                Id = 3,
                Title = "Book 3",
                Author = "Author 3",
                IsReserved = false,
                ReservationComment = null,
            }
        };

        return books;
    }

    private CreateBookDto GetreateBookDto()
    {
        return new CreateBookDto
        {
            Title = "Book 1",
            Author = "Author 1",
        };
    }


    private List<ReservationHistoryDto> GetReservationHistoryDtos()
    {
        List<ReservationHistoryDto> history = new List<ReservationHistoryDto>
        {
            new ReservationHistoryDto
            {
                Id = 1,
                BookId = 1,
                Comment = "Comment: reserving book",
                Event = ReservationAction.Add,
                EventDate = DateTime.Now,
            },
            new ReservationHistoryDto
            {
                Id = 2,
                BookId = 1,
                Comment = "Comment: removing reservation",
                Event = ReservationAction.Remove,
                EventDate = DateTime.Now,
            }
        };

        return history;
    }
}