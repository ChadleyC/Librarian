using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using SupportWave.Librarian.Api.Models;
using SupportWave.Librarian.Api.Services.Books;
using SupportWave.Librarian.Data;
using SupportWave.Librarian.Data.Models;

namespace SupportWave.Librarian.Tests.Services;

public class BookServiceTests : TestBaseFixture
{
    private IBookService _bookService;

    private void Setup(Func<IRepository<Book, Guid>> setupRepo)
    {
        _bookService = new BookService(setupRepo());
    }

    [Fact]
    public void Get()
    {
        // arrange 
        var book = new Book(Guid.NewGuid(), "Wow", "It Works", DateTime.Now, "ISBN-blah");
        Setup(() =>
        {
            var mockRepo = new Mock<IRepository<Book, Guid>>();
            mockRepo.Setup(x => x.Get(It.IsAny<Guid>()))
                .Returns(() => book);
            return mockRepo.Object;
        });

        // act
        var result = _bookService.Get(book.BookId);

        // assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Id.Should().Be(book.BookId);
            Book bookConverted = result;
            bookConverted.Should().BeEquivalentTo(book);
        }
    }

    [Fact]
    public void GetAll()
    {
        // arrange 
        var books = new List<Book>
        {
            new Book(Guid.NewGuid(), "Wow 1", "It Works", DateTime.Now, "ISBN-blah"),
            new Book(Guid.NewGuid(), "Wow 2", "It Works", DateTime.Now, "ISBN-blah"),
            new Book(Guid.NewGuid(), "Wow 3", "It Works", DateTime.Now, "ISBN-blah"),
            new Book(Guid.NewGuid(), "Wow 4", "It Works", DateTime.Now, "ISBN-blah"),
        };
        Setup(() =>
        {
            var mockRepo = new Mock<IRepository<Book, Guid>>();
            mockRepo.Setup(x => x.GetAll())
                .Returns(() => books);
            return mockRepo.Object;
        });

        // act
        var result = _bookService.GetAll();

        // assert
        using (new AssertionScope())
        {
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(books.Count);
            for (int i = 0; i < result.Count; i++)
            {
                Book resultItem = result[i];
                var originItem = books[i];

                resultItem.Should().BeEquivalentTo(originItem);
            }
        }
    }

    public static List<object[]> testDataForSearch = new List<object[]>
    {
        new object[]
        {
            "Wow 1", null, 1,
            new List<Book> { new Book(Guid.NewGuid(), "Wow 1", "It Works", DateTime.Now, "ISBN-blah") }
        },
        new object[]
        {
            null, "It Works", 2, new List<Book>
            {
                new Book(Guid.NewGuid(), "Wow 1", "It Works", DateTime.Now, "ISBN-blah"),
                new Book(Guid.NewGuid(), "Wow 2", "It Works", DateTime.Now, "ISBN-blah")
            }
        },
        new object[]
        {
            "Wow 4", "It Works 1", 1, new List<Book>
            {
                new Book(Guid.NewGuid(), "Wow 4", "It Works 1", DateTime.Now, "ISBN-blah")
            }
        }
    };

    [Theory]
    [MemberData(nameof(testDataForSearch))]
    public void Search(string author, string title, int numberOfRecordsReturned, List<Book> books)
    {
        // arrange 
        Setup(() =>
        {
            var mockRepo = new Mock<IRepository<Book, Guid>>();
            mockRepo.Setup(x => x.GetWhere(It.IsAny<Func<Book, bool>>()))
                .Returns(() => books);
            return mockRepo.Object;
        });

        // act
        var result = _bookService.Search(author, title);

        // assert
        using (new AssertionScope())
        {
            result.Should().NotBeNullOrEmpty();
            foreach (Book bookModel in result)
            {
                var originItem = books.Find(x => x.BookId == bookModel.BookId);
                originItem.Should().NotBeNull();
                originItem.Should().BeEquivalentTo(bookModel);
                originItem.Should().BeEquivalentTo(bookModel);
            }
        }
    }

    [Fact]
    public async Task Insert()
    {
        // arrange 
        var book = new Book(Guid.NewGuid(), "Wow 1", "It Works", DateTime.Now, "ISBN-blah");
        Setup(() =>
        {
            var mockRepo = new Mock<IRepository<Book, Guid>>();
            mockRepo.Setup(x => x.InsertAsync(It.IsAny<Book>()))
                .ReturnsAsync(() => book);
            mockRepo.Setup(x => x.Get(It.IsAny<Guid>()))
                .Returns(book);
            return mockRepo.Object;
        });

        // act
        var result = await _bookService.Insert(new BookModel(book));

        // assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(new BookModel(book));
        }
    }

    [Fact]
    public async Task Update()
    {
        // arrange 
        var book = new Book(Guid.NewGuid(), "Wow 1", "It Works", DateTime.Now, "ISBN-blah");
        Setup(() =>
        {
            var mockRepo = new Mock<IRepository<Book, Guid>>();
            mockRepo.Setup(x => x.UpdateAsync(It.IsAny<Guid>(),It.IsAny<Book>()))
                .ReturnsAsync(() => book);
            mockRepo.Setup(x => x.Get(It.IsAny<Guid>()))
                .Returns(book);
            return mockRepo.Object;
        });

        // act
        var result = await _bookService.Update(book.BookId, new BookModel(book));

        // assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(new BookModel(book));
        }
    }

    [Fact]
    public async Task Delete()
    {
        // arrange 
        var book = new Book(Guid.NewGuid(), "Wow 1", "It Works", DateTime.Now, "ISBN-blah");
        Setup(() =>
        {
            var mockRepo = new Mock<IRepository<Book, Guid>>();
            mockRepo.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => true);
            return mockRepo.Object;
        });

        // act
        var result = await _bookService.Delete(book.BookId);

        // assert
        using (new AssertionScope())
        {
            result.Should().BeTrue();
        }
    }
}