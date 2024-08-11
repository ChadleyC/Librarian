using Librarian.UI.Exceptions;
using Librarian.UI.Models;
using RestSharp;

namespace Librarian.UI.Services;

public class BookService(IRestClient client) : IBookService
{
    public async Task<BookModel?> AddBookAsync(BookModel book)
    {
        try
        {
            var request = new RestRequest("Book", Method.Post);
            request.AddJsonBody(book);
            //var result = await client.PostAsync<BookModel>(request);
            var result = await client.ExecuteAsync(request);
            var finalResult = await client.Deserialize<BookModel>(result, new CancellationToken());
            return finalResult.Data;
        }
        catch (Exception e)
        {
            throw new BookException(e);
        }
    }

    public async Task<BookModel?> EditBookAsync(BookModel book)
    {
        try
        {
            var request = new RestRequest($"/Book/{book.Id}", Method.Put);
            request.AddJsonBody(book);
            return await client.PutAsync<BookModel>(request);
        }
        catch (Exception e)
        {
            throw new BookException(e);
        }
        
    }

    public async Task<List<BookModel>?> GetAllBooksAsync()
    {
        try
        {
            var request = new RestRequest($"/Book/");
            return await client.GetAsync<List<BookModel>>(request);
        }
        catch (Exception e)
        {
            throw new BookException(e);
        }
    }

    public async Task<List<BookModel>?> SearchBooksAsync(string? title, string? author)
    {
        try
        {
            var request = new RestRequest($"/Book/Search?title={title}&author={author}", Method.Get);
            return await client.GetAsync<List<BookModel>>(request) ?? new List<BookModel>();
        }
        catch (Exception e)
        {
            throw new BookException(e);
        }
    }

    public async Task<BookModel?> GetBookAsync(Guid id)
    {
        try
        {
            var request = new RestRequest($"/Book/{id}", Method.Get);
            return await client.GetAsync<BookModel>(request);
        }
        catch (Exception e)
        {
            throw new BookException(e);
        }
    }

    public async Task<bool> DeleteBookAsync(Guid id)
    {
        try
        {
            var request = new RestRequest($"/Book/{id}");
            return await client.DeleteAsync<bool>(request);
        }
        catch (Exception e)
        {
            throw new BookException(e);
        }
    }
}