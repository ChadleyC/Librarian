namespace SupportWave.Librarian.Data.Models
{
	public class Book
	{
		public Book() { }

		public Book(Guid id, string title, string author, DateTime? publishedDate, string isbn)
		{
			this.BookId = id;
			this.Title = title;
			this.Author = author;
			this.PublishedDate = publishedDate;
			this.Isbn = isbn;
		}
		
		public int Id { get; set; }
		public Guid BookId { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Author { get; set; } = string.Empty;
        public DateTime? PublishedDate { get; set; }
		public string Isbn { get; set; } = string.Empty;
    }
}

