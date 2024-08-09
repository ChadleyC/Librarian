using System.ComponentModel.DataAnnotations;
using SupportWave.Librarian.Attributes;
using SupportWave.Librarian.Data.Models;

namespace SupportWave.Librarian.Api.Models
{
	public class BookModel
	{
        public BookModel(){}

        public BookModel(Book? book)
        {
            if (book is null)
            {
                return;
            }

            this.Id = book.BookId;
            this.Title = book.Title;
            this.Author = book.Author;
            this.PublishedDate = book.PublishedDate;
            this.ISBN = book.Isbn;
        }
        
        public BookModel(InsertBookModel? book)
        {
            if (book is null)
            {
                return;
            }

            this.Id = Guid.NewGuid();
            this.Title = book.Title;
            this.Author = book.Author;
            this.PublishedDate = book.PublishedDate;
            this.ISBN = book.Isbn;
        }

        public Guid Id { get; set; }
        [Required]
        public string? Title { get; set; } = string.Empty;
        [Required]
        public string? Author { get; set; } = string.Empty;
        [PastDate(ErrorMessage = "Published Date must be in the past.")]
        public DateTime? PublishedDate { get; set; }
        [Required]
        public string? ISBN { get; set; } = string.Empty;

        public static implicit operator Book(BookModel model)
        {
            return model == null ?
                new Book() :
                new Book(model.Id, model.Title, model.Author, model.PublishedDate, model.ISBN);
        }
    }
}

