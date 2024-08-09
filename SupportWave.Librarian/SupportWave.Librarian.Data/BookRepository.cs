using JsonFlatFileDataStore;
using SupportWave.Librarian.Data.Models;

namespace SupportWave.Librarian.Data
{
    public class BookRepository : IRepository<Book, Guid>
    {
        IDataStore _store;

        public BookRepository(IDataStore dataStore)
        {
            _store = dataStore; //new DataStore("www/book.json");
        }

        ~BookRepository()
        {
            _store.Dispose();
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
            return GetBookCollection().AsQueryable().FirstOrDefault(x => x.BookId.Equals(id));
        }

        /// <summary>
        /// Inserts a book asynchronously, if it fails it will return null
        /// </summary>
        /// <param name="book"></param>
        /// <returns>Book <see cref="Book"/></returns>
        public async Task<Book?> InsertAsync(Book book)
        {
            return await GetBookCollection().InsertOneAsync(book) ? Get(book.BookId) : null;
        }

        /// <summary>
        /// Updates a book asynchronously, returns null if fails
        /// </summary>
        /// <param name="book"></param>
        /// <param name="id"></param>
        /// <returns>Book <see cref="Book"/></returns>
        public async Task<Book?> UpdateAsync(Guid id, Book book)
        {
            var existingBook = Get(id);
            if (existingBook is not null && await GetBookCollection().UpdateOneAsync(book.Id, book))
            {
                return Get(book.BookId);
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
            var book = Get(id);
            return await GetBookCollection().DeleteOneAsync(book?.Id);
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