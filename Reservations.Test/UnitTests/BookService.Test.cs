namespace Reservations.Test.UnitTests;
public class BookServiceTest : IDisposable
{
    private IBookService _bookService;
    private readonly ApplicationDbContext _dbContext;

    public BookServiceTest()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "TestDatabase")
                 .Options;

        _dbContext = new ApplicationDbContext(options);
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
        var logger = new Logger<BookService>(new LoggerFactory());

        _bookService = new BookService(_dbContext, mapper, logger);
    }


    [Fact] // 1.1 CreateAsync with invalid data
    public async Task CreatAsync_WithValidData_ReturnsCreatedBook()
    {
        // Arrange
        var CreatedBook = GetreateBookDto();

        // Act
        var result = await _bookService.CreateAsync(CreatedBook);

        // Assert
        true.Should().BeTrue();
        result.Should().NotBeNull();
        result.Title.Should().Be(CreatedBook.Title);
        result.Author.Should().Be(CreatedBook.Author);
    }


    // 1.2 CreateAsync with valid data
    [Fact]
    public async Task CreatAsync_WithInvalidData_ThrowException()
    {
        // Arrange
        var created = GetreateBookDto();
        created.Title = null!;

        // Act
        var result = async () => await _bookService.CreateAsync(created);

        // Assert
        true.Should().BeTrue();
        await result.Should().ThrowAsync<Exception>();
    }

    // 2.1 GetAllAsync books with no books
    [Fact]
    public async Task GetAllAsync_WithNoBooks_ReturnsEmptyList()
    {
        // Arrange

        // Act
        var result = await _bookService.GetAllAsync();

        // Assert
        true.Should().BeTrue();
        result.Should().NotBeNull();
        result.Should().HaveCount(0);
    }

    // 2.2 Get all books with books
    [Fact]
    public async Task GetAllAsync_WithBooks_ReturnsAllBooks()
    {
        // Arrange
        var createdbook = GetreateBookDto();

        var createdbookdbt = await _bookService.CreateAsync(createdbook);

        // Act
        var result = await _bookService.GetAllAsync();

        // Assert
        true.Should().BeTrue();
        result.Should().NotBeEmpty();
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
    }


    // 3.1 GetByIdAsync with invalid id
    [Fact]
    public async Task GetByIdAsync_WithInvalidId_Null()
    {
        // Arrange
        var invalidId = 100;

        // Act
        var result = await _bookService.GetByIdAsync(invalidId);

        // Assert
        true.Should().BeTrue();
        result.Should().BeNull();
    }


    // 3.2 GetByIdAsync with valid id
    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsBook()
    {
        // Arrange
        var createBood = GetreateBookDto();
        var bookDto = await _bookService.CreateAsync(createBood);
        var validId = bookDto.Id;

        await _bookService.CreateAsync(createBood);

        // Act
        var result = await _bookService.GetByIdAsync(validId);

        // Assert
        true.Should().BeTrue();
        result.Should().NotBeNull();
        result.Id.Should().Be(bookDto.Id);
        result.Title.Should().Be(bookDto.Title);
    }


    // 4.1 UpdateAsync - Update book with invalid id
    [Fact]
    public async Task UpdateAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var invalidId = -100;
        var update = new UpdateBookDto
        {
            Id = invalidId + 1,
            Title = "New Title",
            Author = "New Author",
        };


        // Act
        var result = await _bookService.UpdateAsync(invalidId, update);


        // Assert
        true.Should().BeTrue();
        result.Should().BeNull();
    }


    [Fact] // 4.2 UpdateAsync - Update book with valid id
    public async Task UpdateAsync_WithValidId_ReturnsUpdatedBook()
    {
        // Arrange
        var book = await _bookService.CreateAsync(GetreateBookDto());

        book.Author = "New Author";
        var validId = book.Id;

        var update = new UpdateBookDto
        {
            Id = book.Id,
            Title = "New Title",
            Author = "New Author",
        };


        // Act
        var result = await _bookService.UpdateAsync(validId, update);


        // Assert
        true.Should().BeTrue();
        result.Should().NotBeNull();
        result.Id.Should().Be(book.Id);
        result.Title.Should().Be(update.Title);
    }


    [Fact] // 5.1 DeleteAsync book with invalid id
    public async Task DeleteAsync_WithInvalidId_False()
    {
        // Arrange
        var invalidId = -100;

        // Act
        var result = await _bookService.DeleteAsync(invalidId);

        // Assert
        true.Should().BeTrue();
        result.Should().BeFalse();
    }


    [Fact] // 5.2 DeleteAsync book with valid id
    public async Task DeleteAsync_WithValidId_ReturnsTrue()
    {
        // Arrange
        var createdbook = await _bookService.CreateAsync(GetreateBookDto());
        var validId = createdbook.Id;

        // Act
        var result = await _bookService.DeleteAsync(validId);

        // Assert
        true.Should().BeTrue();
        result.Should().BeTrue();
    }


    [Fact] // 6.1 ReserveBookAsync with invalid id or comment
    public async Task ReserveAsync_WithInvalidIdOrComment_Null()
    {
        // Arrange
        var invalidId = -100;
        var comment = "Comment: reserving book";

        // Act
        var result = await _bookService.ReserveBookAsync(invalidId, comment);

        // Assert
        true.Should().BeTrue();
        result.Should().BeNull();
    }

    [Fact] // 6.2 ReserveBookAsync with valid id and comment
    public async Task ReserveAsync_WithValidIdAndComment_ReturnsReservedBook()
    {
        // Arrange
        var comment = "Comment: reserving book";

        var createdbook = await _bookService.CreateAsync(GetreateBookDto());
        var validId = createdbook.Id;


        // Act
        var result = await _bookService.ReserveBookAsync(validId, comment);


        // Assert
        true.Should().BeTrue();
        result.Should().NotBeNull();
        result.Author.Should().Be(createdbook.Author);
        result.IsReserved.Should().BeTrue();
    }


    [Fact] // 7.1 Remove reservation with invalid id
    public async Task RemoveReservationAsync_WithInvalidId_ReturnsFalse()
    {
        // Arrange
        var invalidId = -100;

        // Act
        var result = await _bookService.RemoveReservationAsync(invalidId);

        // Assert
        true.Should().BeTrue();
        result.Should().BeFalse();
    }

    [Fact] // 7.2 Remove reservation with valid id
    public async Task RemoveReservationAsync_WithValidId_ReturnsTrue()
    {
        // Arrange
        var createdbook = await _bookService.CreateAsync(GetreateBookDto());
        await _bookService.ReserveBookAsync(createdbook.Id, "Comment: reserving book");
        var validId = createdbook.Id;

        // Act
        var result = await _bookService.RemoveReservationAsync(validId);


        // Assert
        true.Should().BeTrue();
        result.Should().BeTrue();
    }

    [Fact] // 8.1 GetAllReservedAsync with no books
    public async Task GetAllReservedAsync_WithNoBooks_ReturnsEmptyList()
    {
        // Arrange

        // Act
        var result = await _bookService.GetReservedBooksAsync();

        // Assert
        true.Should().BeTrue();
        result.Should().HaveCount(0);
    }

    [Fact] // 8.2 GetAllReservedAsync with books
    public async Task GetAllReservedAsync_WithBooks_ReturnsReserverdBooks()
    {
        // Arrange

        var FirstBook = await _bookService.CreateAsync(GetreateBookDto());
        await _bookService.CreateAsync(GetreateBookDto());

        await _bookService.ReserveBookAsync(FirstBook.Id, "Comment: reserving book");

        // Act
        var result = await _bookService.GetReservedBooksAsync();

        // Assert
        true.Should().BeTrue();
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
    }


    [Fact] // 9.1 GetAllAvailableAsync with no books
    public async Task GetAllAvailableAsync_WithNoBooks_ReturnsEmptyList()
    {
        // Arrange

        // Act
        var result = await _bookService.GetAvailableBooksAsync();


        // Assert
        true.Should().BeTrue();
        result.Should().HaveCount(0);
    }

    [Fact] // 9.2 GetAllAvailableAsync with books
    public async Task GetAllAvailableAsync_WithBooks_ReturnsAvailableBooks()
    {
        // Arrange
        var FirstBook = await _bookService.CreateAsync(GetreateBookDto());
        var SecondBook = await _bookService.CreateAsync(GetreateBookDto());
        var ThiredBook = await _bookService.CreateAsync(GetreateBookDto());

        await _bookService.ReserveBookAsync(FirstBook.Id, "Comment: reserving book");

        // Act
        var result = await _bookService.GetAvailableBooksAsync();


        // Assert
        true.Should().BeTrue();
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }


    [Fact] // 10.1 Get books history with no books
    public async Task GetHistoryAsync_WithNoBooks_ReturnsEmptyList()
    {
        // Arrange
        var invalidId = -100;

        // Act
        var result = await _bookService.getSingleBookHistoryAsync(invalidId);


        // Assert
        true.Should().BeTrue();
        result.Should().BeEmpty();
    }

    [Fact] // 10.2 Get books history with books
    public async Task GetHistoryAsync_WithBooks_ReturnsAllBooks()
    {
        // Arrange
        var createdbook = await _bookService.CreateAsync(GetreateBookDto());
        var validId = createdbook.Id;

        await _bookService.ReserveBookAsync(validId, "Comment: reserving book");
        await _bookService.RemoveReservationAsync(validId);


        // Act
        var result = await _bookService.getSingleBookHistoryAsync(validId);


        // Assert
        true.Should().BeTrue();
        result.Should().NotBeEmpty();
        result.Should().HaveCount(2);
        result.Should().BeOfType<List<ReservationHistoryDto>>();
    }




    private CreateBookDto GetreateBookDto()
    {
        return new CreateBookDto
        {
            Title = "Book 1",
            Author = "Author 1",
        };
    }


    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _bookService = null;
        _dbContext?.Dispose();
    }
}
