using SupportWave.Librarian.Api.Models;
using SupportWave.Librarian.Data;
using SupportWave.Librarian.Data.Models;

namespace SupportWave.Librarian.Api.Services.Books
{
	public class BookService : IBookService
	{
        private readonly IRepository<Book, Guid> _bookRepository;

        public BookService(IRepository<Book, Guid> bookRepository)
		{
            this._bookRepository = bookRepository;
        }

        public async Task<bool> Delete(Guid id)
        {
            return await this._bookRepository.DeleteAsync(id);
        }

        public BookModel Get(Guid id)
        {
            var result = this._bookRepository.Get(id) ?? throw new ArgumentException("id provided is invalid");
            return new BookModel(result);
        }

        public List<BookModel> GetAll()
        {
            return this._bookRepository.GetAll().Select(x => new BookModel(x)).ToList();
        }

        public async Task<BookModel> Insert(BookModel book)
        {
            var result = await this._bookRepository.InsertAsync(book) ?? throw new Exception("Error inserting book, please try again");
            return new BookModel(result);
        }

        public List<BookModel> Search(string title, string author)
        {
            bool Where(Book x) => (!string.IsNullOrEmpty(title) && (!x.Title.StartsWith(title, StringComparison.OrdinalIgnoreCase) && !x.Title.Equals(title, StringComparison.OrdinalIgnoreCase))) || (!string.IsNullOrEmpty(author) && (!x.Author.StartsWith(author, StringComparison.OrdinalIgnoreCase) && !x.Author.Equals(author, StringComparison.OrdinalIgnoreCase)));

            return this._bookRepository.GetWhere(Where).Select(x => new BookModel(x)).ToList();
        }

        public async Task<BookModel> Update(Guid id, BookModel book)
        {
            var result = await this._bookRepository.UpdateAsync(id, book) ?? throw new Exception("Error updating book, please try again");
            return new BookModel(result);
        }
    }
}

