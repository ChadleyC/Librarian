using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SupportWave.Librarian.Api.Controllers;
using SupportWave.Librarian.Api.Models;
using SupportWave.Librarian.Api.Services.Books;
using SupportWave.Librarian.Data;
using SupportWave.Librarian.Data.Models;

namespace SupportWave.Librarian.Tests.Controllers;

public class BookControllerTests : TestBaseFixture
{
    private BookController _controller;

    public BookControllerTests() : base()
    {
    }

    private void Setup(Func<IBookService>? getMockBookService = null)
    {
        var bookService = getMockBookService?.Invoke() ?? GetRequiredService<IBookService>();
        _controller = new BookController(bookService);
    }

    private async Task Teardown()
    {
        var repo = GetRequiredService<IRepository<Book, Guid>>();

        var books = repo.GetAll();

        if (!books.Any())
        {
            return;
        }

        foreach (var book in books)
        {
            await repo.DeleteAsync(book.BookId);
        }
    }

    private static Book GetNewBook() =>
        new Book(Guid.NewGuid(), "Test Title", "Test Author", DateTime.Now, "11-2345-66");

    [Fact]
    public async Task WhenIGetAllBooks_Success()
    {
        // arrange
        var book = GetNewBook();
        Setup(() =>
        {
            var mockBookService = new Mock<IBookService>();
            mockBookService.Setup(x => x.GetAll())
                .Returns(() => new List<BookModel>
                {
                    new BookModel(book)
                });
            return mockBookService.Object;
        });

        // act
        var result = _controller.Get();
        var jsonResult = result as JsonResult;

        // assert
        using (new AssertionScope())
        {
            jsonResult.Should().NotBeNull();
            jsonResult!.Value.Should().NotBeNull();
            jsonResult!.Value.Should().BeOfType<List<BookModel>>();
            var list = ((List<BookModel>)jsonResult.Value!);
            list.Count.Should().Be(1);
            list.FirstOrDefault()?.Equals(book);
        }

        // cleanup
        await Teardown();
    }

    [Fact]
    public async Task WhenIGetABook_Success()
    {
        // arrange
        var book = GetNewBook();
        Setup(() =>
        {
            var mockBookService = new Mock<IBookService>();
            mockBookService
                .Setup(x => x.Get(book.BookId))
                .Returns(() => new BookModel(book));
            return mockBookService.Object;
        });

        // act
        var result = _controller.Get(book.BookId);
        var jsonResult = result as JsonResult;

        // assert
        using (new AssertionScope())
        {
            jsonResult.Should().NotBeNull();
            jsonResult!.Value.Should().NotBeNull();
            jsonResult!.Value.Should().BeOfType<BookModel>();
            var bookResult = ((BookModel)jsonResult.Value!) ?? new BookModel();
            bookResult.Should().NotBeNull();
            bookResult.Id.Should().Be(book.BookId);
        }

        // cleanup
        await Teardown();
    }

    [Fact]
    public async Task WhenIPostABook_Success()
    {
        // arrange
        var book = GetNewBook();
        var insertModel = new InsertBookModel
        {
            Author = book.Author,
            Title = book.Title,
            Isbn = book.Isbn,
            PublishedDate = book.PublishedDate
        };
        var bookModel = new BookModel(insertModel);
        Setup(() =>
        {
            var mockBookService = new Mock<IBookService>();
            mockBookService
                .Setup(x => x.Insert(It.IsAny<BookModel>()))
                .Returns(() => Task.FromResult(bookModel));
            return mockBookService.Object;
        });

        // act 
        var result = await _controller.Post(insertModel);
        var jsonResult = result as JsonResult;

        // assert
        using (new AssertionScope())
        {
            jsonResult.Should().NotBeNull();
            jsonResult!.Value.Should().NotBeNull();
            jsonResult!.Value.Should().BeOfType<BookModel>();
            var bookResult = ((BookModel)jsonResult.Value!) ?? new BookModel();
            bookResult.Should().NotBeNull();
            bookResult.Id.Should().Be(bookModel.Id);
        }

        // cleanup
        await Teardown();
    }

    [Fact]
    public async Task WhenIPutABook_Success()
    {
        // arrange
        var book = GetNewBook();
        var bookModel = new BookModel(book);
        Setup(() =>
        {
            var mockBookService = new Mock<IBookService>();
            mockBookService
                .Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<BookModel>()))
                .Returns(() => Task.FromResult(bookModel));
            return mockBookService.Object;
        });

        // act 
        var result = await _controller.Put(bookModel.Id, bookModel);
        var jsonResult = result as JsonResult;

        // assert
        using (new AssertionScope())
        {
            jsonResult.Should().NotBeNull();
            jsonResult!.Value.Should().NotBeNull();
            jsonResult!.Value.Should().BeOfType<BookModel>();
            var bookResult = ((BookModel)jsonResult.Value!) ?? new BookModel();
            bookResult.Should().NotBeNull();
            bookResult.Id.Should().Be(bookModel.Id);
        }

        // cleanup
        await Teardown();
    }

    [Fact]
    public async Task WhenIDeleteABook_Success()
    {
        // arrange
        var book = GetNewBook();
        Setup(() =>
        {
            var mockBookService = new Mock<IBookService>();
            mockBookService.Setup(x => x.Delete(It.IsAny<Guid>()))
                .Returns(() => Task.FromResult(true));
            return mockBookService.Object;
        });

        // act
        var result = await _controller.Delete(book.BookId);
        var jsonResult = result as JsonResult;

        // assert
        using (new AssertionScope())
        {
            jsonResult.Should().NotBeNull();
            jsonResult!.Value.Should().NotBeNull();
            jsonResult!.Value.Should().BeOfType<bool>();
            var deleteResult = (bool)(jsonResult.Value ?? false);
            deleteResult.Should().BeTrue();
        }

        // cleanup
        await Teardown();
    }
}