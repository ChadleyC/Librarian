using SupportWave.Librarian.Data.Models;

namespace SupportWave.Librarian.Api.Models
{
	public class BookModel
	{
        public BookModel(){}

        public BookModel(Book book)
        {
            if (book is null)
            {
                return;
            }

            this.Id = book.Id;
            this.Title = book.Title;
            this.Author = book.Author;
            this.PublishedDate = book.PublishedDate;
            this.ISBN = book.ISBN;
        }

        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public string ISBN { get; set; } = string.Empty;

        public static implicit operator Book(BookModel model)
        {
            return model == null ?
                new Book() :
                new Book(model.Id, model.Title, model.Author, model.PublishedDate, model.ISBN);
        }
    }
}

