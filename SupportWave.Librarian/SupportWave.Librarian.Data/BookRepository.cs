using System;
using System.Linq.Expressions;
using JsonFlatFileDataStore;
using SupportWave.Librarian.Data.Models;

namespace SupportWave.Librarian.Data
{
	public class BookRepository : IRepository<Book, Guid>
	{
		JsonFlatFileDataStore.DataStore _store;

		public BookRepository()
		{
            _store = new DataStore("book.json");
        }

		private IDocumentCollection<Book> GetBookCollection()
			=> _store.GetCollection<Book>();

        /// <summary>
        /// Gets book by Id, if does not exist it will return null
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Book <see cref="Book"/></returns>
        public Book? Get(Guid id)
		{
			return GetBookCollection().AsQueryable().FirstOrDefault(x => x.Id.Equals(id));
		}

        /// <summary>
        /// Inserts a book asynchronously, if it fails it will return null
        /// </summary>
        /// <param name="book"></param>
        /// <returns>Book <see cref="Book"/></returns>
        public async Task<Book?> InsertAsync(Book book)
		{
			if (await GetBookCollection().InsertOneAsync(book))
			{
				return Get(book.Id);
			}

			return null;
        }

        /// <summary>
        /// Updates a book asynchronously, returns null if fails
        /// </summary>
        /// <param name="book"></param>
        /// <returns>Book <see cref="Book"/></returns>
        public async Task<Book?> UpdateAsync(Guid id, Book book)
        {
	        var existingBook = Get(id);
			if (existingBook is not null && await GetBookCollection().UpdateOneAsync(book.Id, book))
			{
				return Get(book.Id);
			}

			return null;
        }

        /// <summary>
        /// Deletes a book asynchronously, returns boolean of succesful or not
        /// </summary>
        /// <param name="id">This is a guid that exists on the book model</param>
        /// <returns>boolean stating whether successful or not</returns>
        public async Task<bool> DeleteAsync(Guid id)
		{
			return await GetBookCollection().DeleteOneAsync(id);
		}

        /// <summary>
        /// Returns all books in collection
        /// </summary>
        /// <returns></returns>
        public List<Book> GetAll()
        {
            return GetWhere(x => true);
        }

        /// <summary>
        /// Returns filtered list of books based on where predicate provided.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Book> GetWhere(Func<Book, bool> where)
        {
            return GetBookCollection().AsQueryable().Where(where).ToList();
        }
    }
}

