using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Reservations.Api.Data;
using Reservations.Api.Services.Implementation;
using Reservations.API.DTO;
using Reservations.API.Model;


namespace Reservations.Test.UnitTests;
public class BookServiceTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllBooks()
    {
        // Arrange
        var dbContextMock = DbContextMocker.GetDbContextMock();
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILogger<BookService>>();

        var bookService = new BookService(dbContextMock.Object, mapperMock.Object, loggerMock.Object);

        var expectedBooks = new List<Book>
        {
            new Book { Id = 1, Title = "Book 1" },
            new Book { Id = 2, Title = "Book 2" },
        };

        dbContextMock.Setup(x => x.Books).Returns(Mock.Of<DbSet<Book>>());

        // Act
        var result = await bookService.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedBooks.Select(b => mapperMock.Object.Map<BookDto>(b)));
    }


    private static class DbContextMocker
    {
        public static Mock<ApplicationDbContext> GetDbContextMock()
        {
            var dbContextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());

            // Add your DbSet mocks here
            dbContextMock.Setup(x => x.Books).Returns(Mock.Of<DbSet<Book>>());
            return dbContextMock;
        }
    }
}
