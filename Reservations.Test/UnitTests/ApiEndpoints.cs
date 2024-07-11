namespace Reservations.Test.UnitTests;

/// <summary>
///     Not currently used.
/// </summary>
public static class ApiEndpoints
{
    private const string BaseUrl = "http://localhost:5099/api/v1";
    private const string BooksBase = BaseUrl + "/books";

    public const string GetAllBooks = BooksBase;
    public const string CreateBook = BooksBase;
    public const string GetReservedBooks = BooksBase + "/reserved-books";
    public const string GetAvailableBooks = BooksBase + "/available-books";

    public static string GetBookById(int id)
    {
        return $"{BooksBase}/{id}";
    }

    public static string UpdateBookById(int id)
    {
        return $"{BooksBase}/{id}";
    }

    public static string DeleteBookById(int id)
    {
        return $"{BooksBase}/{id}";
    }

    public static string ReserveBook(int id, string comment)
    {
        return $"{BooksBase}/{id}/reserve/{comment}";
    }

    public static string RemoveReservation(int id)
    {
        return $"{BooksBase}/{id}/remove-reservation";
    }

    public static string GetReservationHistory(int id)
    {
        return $"{BooksBase}/{id}/history";
    }
}