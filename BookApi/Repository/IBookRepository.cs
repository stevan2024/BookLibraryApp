using Books.Database;
using Books.Models;

namespace Books.Repository
{
    public interface IBookRepository
    {
        public Task<List<Book>> GetBooksAsync();
        public Task<Book> GetBookAsync(long isbn);
        public Task UpdateBookAsync(Book book);
        public Task DeleteBookAsync(long isbn);
        public  Task AddBookAsync(Book book);


    }
}
