using Librarian.UI.Models;

namespace Librarian.UI.Services;

public interface IBookService
{
    Task<BookModel?> AddBookAsync(BookModel book);
    Task<BookModel?> EditBookAsync(BookModel book);
    Task<List<BookModel>?> GetAllBooksAsync();
    Task<List<BookModel>?> SearchBooksAsync(string? title, string? author);
    Task<BookModel?> GetBookAsync(Guid id);
    Task<bool> DeleteBookAsync(Guid id);
}