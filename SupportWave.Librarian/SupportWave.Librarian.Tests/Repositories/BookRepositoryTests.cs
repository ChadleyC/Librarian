using JsonFlatFileDataStore;
using SupportWave.Librarian.Data;
using SupportWave.Librarian.Data.Models;

namespace SupportWave.Librarian.Tests.Repositories;

public class BookRepositoryTests : TestBaseFixture
{
    private IRepository<Book, Guid> _repository;
    
    private void Setup(Func<IDataStore>? setupMockDataStore)
    {
        IDataStore dataStore = setupMockDataStore?.Invoke() ?? GetRequiredService<IDataStore>();
        _repository = new BookRepository(dataStore);
    }
}