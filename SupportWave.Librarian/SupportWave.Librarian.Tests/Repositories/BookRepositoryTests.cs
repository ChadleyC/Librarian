using FluentAssertions;
using FluentAssertions.Execution;
using JsonFlatFileDataStore;
using Moq;
using SupportWave.Librarian.Data;
using SupportWave.Librarian.Data.Models;

namespace SupportWave.Librarian.Tests.Repositories;

public class BookRepositoryTests : TestBaseFixture
{
    private IRepository<Book, Guid>? _repository;

    private void Setup(Func<IDataStore>? setupMockDataStore)
    {
        IDataStore dataStore = setupMockDataStore?.Invoke() ?? GetRequiredService<IDataStore>();
        _repository = new BookRepository(dataStore);
    }

    [Fact]
    public void WhenICallGet_Success()
    {
        // arrange
        var book = new Book(Guid.NewGuid(), "Test Title", "Test Author", DateTime.Now, "TST-ISBN-123");
        var books = new Lazy<List<Book>>();
        books.Value.Add(book);
        Setup(() =>
        {
            var mockDataStore = new Mock<IDataStore>();
            mockDataStore.Setup(x => x.GetCollection<Book>(It.IsAny<string>()))
                .Returns(() => 
                    new DocumentCollection<Book>((p, gd, v) => Task.FromResult(true), 
                        books,
                        "/BooksTest",
                        "id", 
                        (x) => new Book(),
                        () => new Book()));
            return mockDataStore.Object;
        });

        // act
        var result = _repository!.Get(book.BookId);

        // assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().Be(book);
        }
    }

    [Fact]
    public void GetAll()
    {
        // arrange
        var publishedDate = DateTime.Now;
        var bookList = new List<Book>()
        {
            new Book(Guid.NewGuid(), "Test Title 1", "Test Author 1", publishedDate, "TST-ISBN-123-1"),
            new Book(Guid.NewGuid(), "Test Title 2", "Test Author 2", publishedDate, "TST-ISBN-123-2"),
            new Book(Guid.NewGuid(), "Test Title 3", "Test Author 3", publishedDate, "TST-ISBN-123-3"),
            new Book(Guid.NewGuid(), "Test Title 4", "Test Author 4", publishedDate, "TST-ISBN-123-4"),
            new Book(Guid.NewGuid(), "Test Title 5", "Test Author 5", publishedDate, "TST-ISBN-123-5"),
        };
        var books = new Lazy<List<Book>>();
        books.Value.AddRange(bookList);
        Setup(() =>
        {
            var mockDataStore = new Mock<IDataStore>();
            mockDataStore.Setup(x => x.GetCollection<Book>(It.IsAny<string>()))
                .Returns(() => 
                    new DocumentCollection<Book>((p, gd, v) => Task.FromResult(true), 
                        books,
                        "/BooksTest",
                        "id", 
                        (x) => new Book(),
                        () => new Book()));
            return mockDataStore.Object;
        });
        
        // act
        var result = _repository!.GetAll();

        // assert
        using (new AssertionScope())
        {
            result.Should().NotBeNullOrEmpty();
            result.Count.Should().Be(bookList.Count);
            result.Exists(x => !bookList.Contains(x)).Should().BeFalse();
        }
    }

    [Fact]
    public void GetWhere()
    {
        // arrange
        var publishedDate = DateTime.Now;
        var bookList = new List<Book>()
        {
            new Book(Guid.NewGuid(), "Test Title 1", "Test Author 1", publishedDate, "TST-ISBN-123-1"),
            new Book(Guid.NewGuid(), "Test Title 2", "Test Author 2 x", publishedDate, "TST-ISBN-123-2"),
            new Book(Guid.NewGuid(), "Test Title 3", "Test Author 3", publishedDate, "TST-ISBN-123-3"),
            new Book(Guid.NewGuid(), "Test Title 4", "Test Author 4 x", publishedDate, "TST-ISBN-123-4"),
            new Book(Guid.NewGuid(), "Test Title 5", "Test Author 5", publishedDate, "TST-ISBN-123-5"),
        };
        var subset = bookList.Where(x => x.Author.Contains("x")).ToList();
        var books = new Lazy<List<Book>>();
        books.Value.AddRange(bookList);
        Setup(() =>
        {
            var mockDataStore = new Mock<IDataStore>();
            mockDataStore.Setup(x => x.GetCollection<Book>(It.IsAny<string>()))
                .Returns(() => 
                    new DocumentCollection<Book>((p, gd, v) => Task.FromResult(true), 
                        books,
                        "/BooksTest",
                        "id", 
                        (x) => new Book(),
                        () => new Book()));
            return mockDataStore.Object;
        });

        // act
        var result = _repository!.GetWhere(x => x.Author?.Contains("x") ?? false);

        // assert
        using (new AssertionScope())
        {
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(2);
            result.Exists(x => !subset.Contains(x)).Should().BeFalse();
        }
    }

    [Fact]
    public async Task InsertAsync()
    {
        // arrange
        var book = new Book(Guid.NewGuid(), "Test Title", "Test Author", DateTime.Now, "TST-ISBN-123");
        var books = new Lazy<List<Book>>();
        books.Value.Add(book);
        Setup(() =>
        {
            var mockDataStore = new Mock<IDataStore>();
            mockDataStore.Setup(x => x.GetCollection<Book>(It.IsAny<string>()))
                .Returns(() => 
                    new DocumentCollection<Book>((p, gd, v) => Task.FromResult(true), 
                        books,
                        "/BooksTest",
                        "id", 
                        (x) => new Book(),
                        () => new Book()));
            return mockDataStore.Object;
        });

        // act
        var result = await _repository!.InsertAsync(book);

        // assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().Be(book);
        }
    }

    [Fact]
    public async Task UpdateAsync()
    {
        // arrange
        var book = new Book(Guid.NewGuid(), "Test Title", "Test Author", DateTime.Now, "TST-ISBN-123");
        var updatedBook = new Book(book.BookId, book.Title, book.Author, book.PublishedDate, book.Isbn);
        updatedBook.PublishedDate = DateTime.Now;
        
        var books = new Lazy<List<Book>>();
        books.Value.Add(updatedBook);
        Setup(() =>
        {
            var mockDataStore = new Mock<IDataStore>();
            mockDataStore.Setup(x => x.GetCollection<Book>(It.IsAny<string>()))
                .Returns(() => 
                    new DocumentCollection<Book>((p, gd, v) => Task.FromResult(true), 
                        books,
                        "/BooksTest",
                        "id", 
                        (x) => new Book(),
                        () => new Book()));
            return mockDataStore.Object;
        });

        // act
        var result = await _repository.UpdateAsync(updatedBook.BookId, updatedBook);

        // assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().Be(updatedBook);
        }
    }

    [Fact]
    public async Task DeleteAsync()
    {
        // arrange
        var book = new Book(Guid.NewGuid(), "Test Title", "Test Author", DateTime.Now, "TST-ISBN-123");
        var books = new Lazy<List<Book>>();
        books.Value.Add(book);
        Setup(() =>
        {
            var mockDataStore = new Mock<IDataStore>();
            mockDataStore.Setup(x => x.GetCollection<Book>(It.IsAny<string>()))
                .Returns(() => 
                    new DocumentCollection<Book>((p, gd, v) => Task.FromResult(true), 
                        books,
                        "/BooksTest",
                        "id", 
                        (x) => new Book(),
                        () => new Book()));
            return mockDataStore.Object;
        });

        // act
        var result = await _repository.DeleteAsync(book.BookId);

        // assert
        using (new AssertionScope())
        {
            result.Should().BeTrue();
        }
    }
}