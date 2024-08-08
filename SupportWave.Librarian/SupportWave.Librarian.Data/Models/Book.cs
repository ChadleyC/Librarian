namespace SupportWave.Librarian.Data.Models
{
	public class Book
	{
		public Book() { }

		public Book(Guid id, string title, string author, DateTime publishedDate, string isbn)
		{
			this.Id = id;
			this.Title = title;
			this.Author = author;
			this.PublishedDate = publishedDate;
			this.ISBN = isbn;
		}

		public Guid Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Author { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
		public string ISBN { get; set; } = string.Empty;
    }
}

