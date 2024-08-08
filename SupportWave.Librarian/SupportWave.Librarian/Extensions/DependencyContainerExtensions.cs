using SupportWave.Librarian.Api.Services.Books;
using SupportWave.Librarian.Data;
using SupportWave.Librarian.Data.Models;

namespace SupportWave.Librarian.Extensions;

public static class DependencyContainerExtensions
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddTransient<IRepository<Book, Guid>, BookRepository>();
        services.AddTransient<IBookService, BookService>();
    }
    
}