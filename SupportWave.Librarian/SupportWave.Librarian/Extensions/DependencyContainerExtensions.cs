using JsonFlatFileDataStore;
using SupportWave.Librarian.Api.Services.Books;
using SupportWave.Librarian.Data;
using SupportWave.Librarian.Data.Models;

namespace SupportWave.Librarian.Api.Extensions;

public static class DependencyContainerExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<IDataStore, DataStore>(ImplementationFactory);
        services.AddTransient<IRepository<Book, Guid>, BookRepository>();
        services.AddTransient<IBookService, BookService>();
    }

    private static DataStore ImplementationFactory(IServiceProvider arg)
    {
        return new DataStore("www/books.json");
    }
}