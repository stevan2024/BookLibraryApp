
using Books.Database;
using Books.Exceptions;
using Books.Models;
using Books.Services;
using Microsoft.EntityFrameworkCore;

namespace Books.Repository
{


    public class BookRepository : IBookRepository
    {
        BooksDBContext _booksDBContext;
        IIsbnChecker _isbnChecker;

        /// <summary>
        /// Book Repository for CRUD operations.
        /// </summary>
        /// <param name="booksDBContext">DB Context</param>
        /// <param name="isbnChecker">isbn checker service</param>
        public BookRepository(BooksDBContext booksDBContext, IIsbnChecker isbnChecker)
        {
            _booksDBContext = booksDBContext;
            _isbnChecker = isbnChecker;
        }


        /// <summary>
        /// Get All Books
        /// </summary>
        /// <returns>List of books</returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<List<Book>> GetBooksAsync()
        {
            List<Book> books = await _booksDBContext.Books.ToListAsync();

            if (books == null || books.Count == 0) throw new NotFoundException("No Books Found");

            return books;
        }

        /// <summary>
        /// Get one Book
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns>Book</returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<Book> GetBookAsync(long isbn)
        {
            var book = await _booksDBContext.Books.FindAsync(isbn);

            if (book == null) throw new NotFoundException("No Book Found");

            return book;

        }

        /// <summary>
        /// Update a Book
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task UpdateBookAsync(Book book)
        {

            var foundBook = await _booksDBContext.Books.FindAsync(book.ISBN);
            if (foundBook == null) throw new NotFoundException("No Book Found to update");

            _booksDBContext.Books.Update(book);
            await _booksDBContext.SaveChangesAsync();
            return;
        }


        /// <summary>
        /// Add a book
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        /// <exception cref="BookAlreadyExistsException"></exception>
        /// <exception cref="NotValidIsbnException"></exception>
        public async Task AddBookAsync(Book book)
        {
            var foundBook = await _booksDBContext.Books.FindAsync(book.ISBN);

            if (foundBook != null) throw new BookAlreadyExistsException("Book Already Exists");

            if (!_isbnChecker.isValidISBNCode(book.ISBN.ToString())) throw new NotValidIsbnException("Not Valid ISBN");

            _booksDBContext.Books.Add(book);
            await _booksDBContext.SaveChangesAsync();
            return;

        }

        /// <summary>
        /// Delete a book
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task DeleteBookAsync(long isbn)
        {

            Book book = await _booksDBContext.Books.SingleOrDefaultAsync(x => x.ISBN == isbn);

            if (book == null) throw new NotFoundException("No Book Found to delete");
            _booksDBContext.Books.Remove(book);
            await _booksDBContext.SaveChangesAsync();
            return;

        }



    }
}
