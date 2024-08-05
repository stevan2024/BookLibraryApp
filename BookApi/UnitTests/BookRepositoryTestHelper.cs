using Books.Database;
using Books.Models;
using Microsoft.EntityFrameworkCore;

namespace Books.UnitTests
{
    public class BookRepositoryTestHelper
    {
        /// <summary>
        /// Obtain a reference to the db context for testing
        /// </summary>
        /// <returns></returns>
        public BooksDBContext GetMemoryContext()
        {
            var options = new DbContextOptionsBuilder<BooksDBContext>()
            .UseInMemoryDatabase(databaseName: "Books")
            .Options;
            return new BooksDBContext(options);
        }


        /// <summary>
        /// Use a DB context which has all the books for testing
        /// </summary>
        /// <returns></returns>
        public BooksDBContext PopulateAllBooksInDBContent()
        {
            BooksDBContext booksDBContext = GetMemoryContext();

            // clear out any existing Books
            booksDBContext.Books.RemoveRange(booksDBContext.Books);
            booksDBContext.SaveChanges();

            // add books for test
            List<Book> expectedBooks = new()
            {
                new Book()
                {
                    ISBN = 9780671612979,
                    Title = "Ben Hogan's Five Lessons: The Modern Fundamentals of Golf",
                    Authors = "Ben Hogan; Herbert Warren Wind",
                    Publisher = "Touchstone"

                },
                new Book()
                {
                    ISBN = 9781401309619,
                    Title = "The Match: The Day the Game of Golf Changed Forever",
                    Authors = "Mark Frost",
                    Publisher = "Hachette Books"

                }
            };


            booksDBContext.Books.AddRange(expectedBooks);
            booksDBContext.SaveChanges();

            return booksDBContext;
        }

        /// <summary>
        /// Use a DB context which has no books for testing
        /// </summary>
        /// <returns></returns>
        public BooksDBContext PopulateNoBooksInDBContent()
        {
            BooksDBContext booksDBContext = GetMemoryContext();

            // clear out any existing Books
            booksDBContext.Books.RemoveRange(booksDBContext.Books);
            booksDBContext.SaveChanges();

            // add books for test
            List<Book> expectedBooks = new();

            booksDBContext.Books.AddRange(expectedBooks);
            booksDBContext.SaveChanges();

            return booksDBContext;
        }


    }
}
