using System;
using SupportWave.Librarian.Api.Models;

namespace SupportWave.Librarian.Api.Services.Books
{
	public interface IBookService
	{
		BookModel Get(Guid id);
		List<BookModel> GetAll();
		List<BookModel> Search(string title, string author);
		Task<BookModel> Insert(BookModel book);
		Task<BookModel> Update(Guid id, BookModel book);
		Task<bool> Delete(Guid id);
	}
}

