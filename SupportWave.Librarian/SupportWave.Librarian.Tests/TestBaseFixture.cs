using JsonFlatFileDataStore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SupportWave.Librarian.Api.Services.Books;
using SupportWave.Librarian.Data;
using SupportWave.Librarian.Data.Models;

namespace SupportWave.Librarian.Tests;

public class TestBaseFixture
{
    private IServiceProvider _provider;

    public TestBaseFixture()
    {
        _provider = Provider();
    }

    public TestBaseFixture(bool mockDataStore, bool mockRepo, bool mockService)
    {
        _provider = Provider();
    }

    private IServiceProvider Provider()
    {
        var services = new ServiceCollection();
        services.AddTransient<IDataStore, DataStore>(x => new DataStore("books.json"));
        services.AddTransient<IRepository<Book, Guid>, BookRepository>();
        services.AddTransient<IBookService, BookService>();

        return services.BuildServiceProvider();
    }

    protected T GetRequiredService<T>() where T : notnull
    {
        return _provider.GetRequiredService<T>();
    }
}