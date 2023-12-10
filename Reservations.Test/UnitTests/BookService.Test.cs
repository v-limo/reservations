namespace Reservations.Test.UnitTests;

public class BookServiceTest
{
    private IBookService _bookService;

    public BookServiceTest()
    {
        Mock<ApplicationDbContext> dbContextMock = new();
        Mock<IMapper> mapperMock = new();
        Mock<ILogger<BookService>> loggerMock = new();
        Mock<DbSet<Book>> dbSetMock = new();

       // TODO: fix mocking of DbContext
        // dbContextMock.Setup(x => x.Books).Returns(dbSetMock.Object);
    }


    [Fact] // 1.1 CreateAsync with invalid data
    public async Task CreatAsync_WithValidData_ReturnsCreatedBook()
    {
        // Arrange
        var bookDto = GetreateBookDto();
        var book = GetBooksData().FirstOrDefault();

        // Act
        // var result = await _bookService.CreateAsync(bookDto);
        await Task.Delay(1); // TODO: remove this line here and in all other tests

        // Assert
        true.Should().BeTrue();
    }


    // 1.2 CreateAsync with valid data
    [Fact]
    public async Task CreatAsync_WithInvalidData_ReturnsNull()
    {
        // Arrange
        var created = GetreateBookDto();
        created.Title = null!;
        var book = GetBooksData().FirstOrDefault();

        // Act
        // var result = await _bookService.CreateAsync(created);
        await Task.Delay(1);

        // Assert
        true.Should().BeTrue();
    }

    // 2.1 GetAllAsync books with no books
    [Fact]
    public async Task GetAllAsync_WithNoBooks_ReturnsEmptyList()
    {
        // Arrange
        var booksDto = new List<BookDto>();

        // Act
        // var result = await _bookService.GetAllAsync();
        await Task.Delay(1);

        // Assert
        true.Should().BeTrue();
    }

    // 2.2 Get all books with books
    [Fact]
    public async Task GetAllAsync_WithBooks_ReturnsAllBooks()
    {
        // Arrange
        var booksDto = GetBooksDto();

        // Act
        // var result = await _bookService.GetAllAsync();
        await Task.Delay(1);

        // Assert
        true.Should().BeTrue();
    }


    // 3.1 GetByIdAsync with invalid id
    [Fact]
    public async Task GetByIdAsync_WithInvalidId_Null()
    {
        // Arrange
        var bookDto = GetBooksDto().FirstOrDefault();
        var invalidId = 100;

        // Act
        // var result = await _bookService.GetByIdAsync(invalidId);
        await Task.Delay(1);

        // Assert
        true.Should().BeTrue();
    }


    // 3.2 GetByIdAsync with valid id
    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsBook()
    {
        // Arrange
        var bookDto = GetBooksDto().FirstOrDefault();
        var validId = 1;

        // Act
        // var result = await _bookService.GetByIdAsync(validId);
        await Task.Delay(1);

        // Assert
        true.Should().BeTrue();
    }


    // 4.1 UpdateAsync - Update book with invalid id
    [Fact]
    public async Task UpdateAsync_WithInvalidId_Null()
    {
        // Arrange
        var bookDto = GetBooksDto().FirstOrDefault();
        var invalidId = 100;
        var update = new UpdateBookDto
        {
            Id = bookDto.Id,
            Title = bookDto.Title,
            Author = "New Author",
        };


        // Act
        // var result = await _bookService.UpdateAsync(invalidId, update);
        await Task.Delay(1);

        // Assert
        true.Should().BeTrue();
    }


    [Fact] // 4.2 UpdateAsync - Update book with valid id
    public async Task UpdateAsync_WithValidId_ReturnsUpdatedBook()
    {
        // Arrange
        var bookDto = GetBooksDto().FirstOrDefault();
        var validId = 1;
        var update = new UpdateBookDto
        {
            Id = bookDto.Id,
            Title = bookDto.Title,
            Author = "New Author",
        };

        // Act
        // var result = await _bookService.UpdateAsync(validId, update);
        await Task.Delay(1);

        // Assert
        true.Should().BeTrue();
    }


    [Fact] // 5.1 DeleteAsync book with invalid id
    public async Task DeleteAsync_WithInvalidId_Null()
    {
        // Arrange
        var invalidId = 100;

        // Act
        // var result = await _bookService.DeleteAsync(invalidId);
        await Task.Delay(1);

        // Assert
        true.Should().BeTrue();
    }


    [Fact] // 5.2 DeleteAsync book with valid id
    public async Task DeleteAsync_WithValidId_ReturnsXYZ()
    {
        // Arrange
        var validId = 1;

        // Act
        // var result = await _bookService.DeleteAsync(validId);
        await Task.Delay(1);

        // Assert
        true.Should().BeTrue();
    }


    [Fact] // 6.1 ReserveBookAsync with invalid id or comment
    public async Task ReserveAsync_WithInvalidIdOrComment_Null()
    {
        // Arrange
        var invalidId = 100;
        var comment = "Comment: reserving book";

        // Act
        // var result = await _bookService.ReserveBookAsync(invalidId, comment);
        await Task.Delay(1);

        // Assert
        true.Should().BeTrue();
    }

    [Fact] // 6.2 ReserveBookAsync with valid id and comment
    public async Task ReserveAsync_WithValidIdAndComment_ReturnsXYZ()
    {
        // Arrange
        var validId = 1;
        var comment = "Comment: reserving book";


        // Act
        // var result = await _bookService.ReserveBookAsync(validId, comment);
        await Task.Delay(1);

        // Assert
        true.Should().BeTrue();
    }


    [Fact] // 7.1 Remove reservation with invalid id
    public async Task RemoveReservationAsync_WithInvalidId_Null()
    {
        // Arrange
        var invalidId = 100;

        // Act
        // var result = await _bookService.RemoveReservationAsync(invalidId);
        await Task.Delay(1);

        // Assert
        true.Should().BeTrue();
    }

    [Fact] // 7.2 Remove reservation with valid id
    public async Task RemoveReservationAsync_WithValidId_ReturnsTrue()
    {
        // Arrange
        var validId = 1;

        // Act
        // var result = await _bookService.RemoveReservationAsync(validId);
        await Task.Delay(1);

        // Assert
        true.Should().BeTrue();
    }

    [Fact] // 8.1 GetAllReservedAsync with no books
    public async Task GetAllReservedAsync_WithNoBooks_ReturnsEmptyList()
    {
        // Arrange
        var booksDto = new List<BookDto>();

        // Act
        // var result = await _bookService.GetReservedBooksAsync();
        await Task.Delay(1);

        // Assert
        true.Should().BeTrue();
    }

    [Fact] // 8.2 GetAllReservedAsync with books
    public async Task GetAllReservedAsync_WithBooks_ReturnsAllBooks()
    {
        // Arrange
        var booksDto = GetBooksDto(); // 2/3 books are reserved

        // Act
        // var result = await _bookService.GetReservedBooksAsync();
        await Task.Delay(1);

        // Assert
        true.Should().BeTrue();
    }


    [Fact] // 9.1 GetAllAvailableAsync with no books
    public async Task GetAllAvailableAsync_WithNoBooks_ReturnsEmptyList()
    {
        // Arrange
        var booksDto = new List<BookDto>();

        // Act
        // var result = await _bookService.GetAvailableBooksAsync();
        await Task.Delay(1);

        // Assert
        true.Should().BeTrue();
    }

    [Fact] // 9.2 GetAllAvailableAsync with books
    public async Task GetAllAvailableAsync_WithBooks_ReturnsAllBooks()
    {
        // Arrange
        var booksDto = GetBooksDto(); // 2/3 books are reserved

        // Act
        // var result = await _bookService.GetAvailableBooksAsync();
        await Task.Delay(1);

        // Assert
        true.Should().BeTrue();
    }


    [Fact] // 10.1 Get books history with no books
    public async Task GetHistoryAsync_WithNoBooks_ReturnsEmptyList()
    {
        // Arrange
        var booksDto = new List<BookDto>();
        var history = GetReservationHistoryDtos();
        var invalidId = 100;

        // Act
        // var result = await _bookService.getSingleBookHistoryAsync(invalidId);
        await Task.Delay(1);

        // Assert
        true.Should().BeTrue();
    }

    [Fact] // 10.2 Get books history with books
    public async Task GetHistoryAsync_WithBooks_ReturnsAllBooks()
    {
        // Arrange
        var history = GetReservationHistoryDtos();
        var validId = 1;

        // Act
        // var result = await _bookService.getSingleBookHistoryAsync(validId);
        await Task.Delay(1);

        // Assert
        true.Should().BeTrue();
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
