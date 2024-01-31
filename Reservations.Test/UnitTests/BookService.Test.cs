namespace Reservations.Test.UnitTests;

public class BookServiceTest : IDisposable
{
    private readonly ApplicationDbContext _dbContext;
    private IBookService _bookService;

    public BookServiceTest()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
        var logger = new Logger<BookService>(new LoggerFactory());

        _bookService = new BookService(_dbContext, mapper, logger);
    }


    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _bookService = null!;
        _dbContext.Dispose();
    }


    // 1 CreateAsync
    [Fact]
    public async Task CreatAsync_WithValidData_ReturnsCreatedBook()
    {
        // Arrange
        var createdBook = GreateBookDto();

        // Act
        var result = await _bookService.CreateAsync(createdBook);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(createdBook.Title);
        result.Author.Should().Be(createdBook.Author);
    }

    [Fact]
    public async Task CreatAsync_WithInvalidData_ThrowException()
    {
        // Arrange
        var created = GreateBookDto();
        created.Title = null!;

        // Act
        var result = async () => await _bookService.CreateAsync(created);

        // Assert
        await result.Should().ThrowAsync<Exception>();
    }

    // 2.1 GetAllAsync
    [Fact]
    public async Task GetAllAsync_WithNoBooks_ReturnsEmptyList()
    {
        // Act
        var result = await _bookService.GetAllAsync();

        // Assert
        var bookDtos = result as BookDto[] ?? result.ToArray();
        bookDtos.Should().NotBeNull();
        bookDtos.Should().HaveCount(0);
    }


    [Fact]
    public async Task GetAllAsync_WithBooks_ReturnsAllBooks()
    {
        // Arrange
        var createdBook = GreateBookDto();
        await _bookService.CreateAsync(createdBook);

        // Act
        var result = await _bookService.GetAllAsync();

        // Assert
        var bookDto = result as BookDto[] ?? result.ToArray();
        bookDto.Should().NotBeEmpty();
        bookDto.Should().NotBeNull();
        bookDto.Should().HaveCount(1);
    }

    // 3. GetByIdAsync
    [Fact]
    public async Task GetByIdAsync_WithInvalidId_Null()
    {
        // Arrange
        const int invalidId = 100;

        // Act
        var result = await _bookService.GetByIdAsync(invalidId);

        // Assert
        result.Should().BeNull();
    }


    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsBook()
    {
        // Arrange
        var createdBook = GreateBookDto();
        var bookDto = await _bookService.CreateAsync(createdBook);
        var validId = bookDto.Id;

        await _bookService.CreateAsync(createdBook);

        // Act
        var result = await _bookService.GetByIdAsync(validId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(bookDto.Id);
        result.Title.Should().Be(bookDto.Title);
    }


    // 4. UpdateAsync
    [Fact]
    public async Task UpdateAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        const int invalidId = -100;
        var update = new UpdateBookDto
        {
            Id = invalidId + 1,
            Title = "New Title",
            Author = "New Author"
        };

        // Act
        var result = await _bookService.UpdateAsync(invalidId, update);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_WithValidId_ReturnsUpdatedBook()
    {
        // Arrange
        var book = await _bookService.CreateAsync(GreateBookDto());

        book.Author = "New Author";
        var validId = book.Id;

        var update = new UpdateBookDto
        {
            Id = book.Id,
            Title = "New Title",
            Author = "New Author"
        };

        // Act
        var result = await _bookService.UpdateAsync(validId, update);

        // Assert
        result.Should().NotBeNull();
        result?.Id.Should().Be(book.Id);
        result?.Title.Should().Be(update.Title);
    }


    [Fact]
    public async Task DeleteAsync_WithInvalidId_False()
    {
        // Arrange
        const int invalidId = -100;

        // Act
        var result = await _bookService.DeleteAsync(invalidId);

        // Assert
        result.Should().BeFalse();
    }


    [Fact]
    public async Task DeleteAsync_WithValidId_ReturnsTrue()
    {
        // Arrange
        var createdBook = await _bookService.CreateAsync(GreateBookDto());
        var validId = createdBook.Id;

        // Act
        var result = await _bookService.DeleteAsync(validId);

        // Assert
        result.Should().BeTrue();
    }

    // 6. ReserveBookAsync
    [Fact]
    public async Task ReserveAsync_WithInvalidIdOrComment_Null()
    {
        // Arrange
        const int invalidId = -100;
        const string comment = "Comment: reserving book";

        // Act
        var result = await _bookService.ReserveBookAsync(invalidId, comment);

        // Assert
        result.Should().BeNull();
    }

    // 6. ReserveBookAsync
    [Fact]
    public async Task ReserveAsync_WithValidIdAndComment_ReturnsReservedBook()
    {
        // Arrange
        const string comment = "Comment: reserving book";
        var createdBook = await _bookService.CreateAsync(GreateBookDto());
        var validId = createdBook.Id;

        // Act
        var result = await _bookService.ReserveBookAsync(validId, comment);

        // Assert
        result.Should().NotBeNull();
        result?.Author.Should().Be(createdBook.Author);
        result?.IsReserved.Should().BeTrue();
    }

    // 7. Remove reservation
    [Fact]
    public async Task RemoveReservationAsync_WithInvalidId_ReturnsFalse()
    {
        // Arrange
        const int invalidId = -100;

        // Act
        var result = await _bookService.RemoveReservationAsync(invalidId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task RemoveReservationAsync_WithValidId_ReturnsTrue()
    {
        // Arrange
        var createdBook = await _bookService.CreateAsync(GreateBookDto());
        await _bookService.ReserveBookAsync(createdBook.Id, "Comment: reserving book");
        var validId = createdBook.Id;

        // Act
        var result = await _bookService.RemoveReservationAsync(validId);

        // Assert
        result.Should().BeTrue();
    }

    // 8 GetAllReservedAsync
    [Fact]
    public async Task GetAllReservedAsync_WithNoBooks_ReturnsEmptyList()
    {
        // Act
        var result = await _bookService.GetReservedBooksAsync();

        // Assert
        result.Should().HaveCount(0);
    }

    [Fact]
    public async Task GetAllReservedAsync_WithBooks_ReturnsReserverdBooks()
    {
        // Arrange
        var firstBook = await _bookService.CreateAsync(GreateBookDto());
        await _bookService.CreateAsync(GreateBookDto());
        await _bookService.ReserveBookAsync(firstBook.Id, "Comment: reserving book");

        // Act
        var result = await _bookService.GetReservedBooksAsync();

        // Assert
        IEnumerable<BookDto> bookDtos = result.ToList();
        bookDtos.Should().NotBeNull();
        bookDtos.Should().HaveCount(1);
    }

    // 9. GetAllAvailableAsync
    [Fact]
    public async Task GetAllAvailableAsync_WithNoBooks_ReturnsEmptyList()
    {
        // Act
        var result = await _bookService.GetAvailableBooksAsync();

        // Assert
        result.Should().HaveCount(0);
    }

    [Fact]
    public async Task GetAllAvailableAsync_WithBooks_ReturnsAvailableBooks()
    {
        // Arrange
        var firstBook = await _bookService.CreateAsync(GreateBookDto());
        var secondBook = await _bookService.CreateAsync(GreateBookDto());
        var thirdBook = await _bookService.CreateAsync(GreateBookDto());

        await _bookService.ReserveBookAsync(firstBook.Id, "Comment: reserving book");

        // Act
        var result = await _bookService.GetAvailableBooksAsync();

        // Assert
        IEnumerable<BookDto> bookDtos = result as BookDto[] ?? result.ToArray();
        bookDtos.Should().NotBeNull();
        bookDtos.Should().HaveCount(2);
    }

    // 10 Get books history
    [Fact]
    public async Task GetHistoryAsync_WithNoBooks_ReturnsEmptyList()
    {
        // Arrange
        const int invalidId = -100;

        // Act
        var result = await _bookService.getSingleBookHistoryAsync(invalidId);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetHistoryAsync_WithBooks_ReturnsAllBooks()
    {
        // Arrange
        var createdBook = await _bookService.CreateAsync(GreateBookDto());
        var validId = createdBook.Id;

        await _bookService.ReserveBookAsync(validId, "Comment: reserving book");
        await _bookService.RemoveReservationAsync(validId);

        // Act
        var result = await _bookService.getSingleBookHistoryAsync(validId);

        // Assert
        var historyDtos = result.ToList();
        historyDtos.Should().NotBeEmpty();
        historyDtos.Should().HaveCount(2);
        historyDtos.Should().BeOfType<List<ReservationHistoryDto>>();
    }

    private static CreateBookDto GreateBookDto()
    {
        return new CreateBookDto
        {
            Title = "Book 1",
            Author = "Author 1"
        };
    }
}