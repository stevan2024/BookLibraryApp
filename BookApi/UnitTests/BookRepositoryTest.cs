
using Moq;
using Books.Repository;
using Books.Database;
using Books.Models;
using Books.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Books.Exceptions;

namespace Books.UnitTests
{
    [TestClass]
    public class BookRepositoryTest
    {
        private BooksDBContext? _booksDBContext;
        Mock<IIsbnChecker> _mockIsbnChecker = new();
        private BookRepositoryTestHelper _bookRepositoryTestHelper = new();

        [TestInitialize]
        public void testInit()
        {
            _bookRepositoryTestHelper = new BookRepositoryTestHelper();
        }

        /// <summary>
        /// Success path for GetBooks
        /// </summary>
        [TestMethod]
        public async Task GetBooksAsync_SuccessAsync()
        {
            //Arrange
            _mockIsbnChecker = new Mock<IIsbnChecker>();
            _mockIsbnChecker.Setup(m => m.isValidISBNCode(It.IsAny<String>())).Returns(true);
            _booksDBContext = _bookRepositoryTestHelper.PopulateAllBooksInDBContent();

            //Act
            BookRepository bookRepo = new BookRepository(_booksDBContext, _mockIsbnChecker.Object);
            var actualBooks = await bookRepo.GetBooksAsync();

            //Assert
            Assert.IsNotNull(actualBooks);
            Assert.IsTrue(actualBooks.Count == 2);
        }

        /// <summary>
        /// GetBooks with no books in DB context
        /// </summary>
        [TestMethod]
        public async Task GetBooksAsync_NoBooksFailAsync()
        {
            //Arrange
            _mockIsbnChecker = new Mock<IIsbnChecker>();
            _mockIsbnChecker.Setup(m => m.isValidISBNCode(It.IsAny<String>())).Returns(true);
            _booksDBContext = _bookRepositoryTestHelper.PopulateNoBooksInDBContent();


            //Act
            BookRepository bookRepo = new BookRepository(_booksDBContext, _mockIsbnChecker.Object);
            try
            {
                var actualBooks = await bookRepo.GetBooksAsync();
            }
            catch (NotFoundException exception)
            {
                //Assert
                Assert.IsTrue(exception.Message == "No Books Found");
            }


        }


        /// <summary>
        /// GetBook success path
        /// </summary>
        [TestMethod]
        public async Task GetBookAsync_SuccessAsync()
        {
            //Arrange
            _mockIsbnChecker = new Mock<IIsbnChecker>();
            _mockIsbnChecker.Setup(m => m.isValidISBNCode(It.IsAny<String>())).Returns(true);
            _booksDBContext = _bookRepositoryTestHelper.PopulateAllBooksInDBContent();

            //Act
            BookRepository bookRepo = new BookRepository(_booksDBContext, _mockIsbnChecker.Object);
            var actualBook = await bookRepo.GetBookAsync(9781401309619);

            //Assert
            Assert.IsNotNull(actualBook);
            Assert.IsTrue(actualBook.ISBN == 9781401309619);
        }


        /// <summary>
        /// GetBook -  book not found
        /// </summary>
        [TestMethod]
        public async Task GetBookAsync_NoBookFailAsync()
        {
            //Arrange
            _mockIsbnChecker = new Mock<IIsbnChecker>();
            _mockIsbnChecker.Setup(m => m.isValidISBNCode(It.IsAny<String>())).Returns(true);
            _booksDBContext = _bookRepositoryTestHelper.PopulateAllBooksInDBContent();

            //Act
            BookRepository bookRepo = new BookRepository(_booksDBContext, _mockIsbnChecker.Object);

            try
            {
                var actualBook = await bookRepo.GetBookAsync(123);
            }
            catch (NotFoundException exception)
            {
                //Asert
                Assert.IsTrue(exception.Message == "No Book Found");
            }


        }

        /// <summary>
        /// UpdateBook -  success
        /// </summary>
        [TestMethod]
        public async Task UpdateBookAsync_SuccessAsync()
        {
            //Arrange
            _mockIsbnChecker = new Mock<IIsbnChecker>();
            _mockIsbnChecker.Setup(m => m.isValidISBNCode(It.IsAny<String>())).Returns(true);
            _booksDBContext = _bookRepositoryTestHelper.PopulateAllBooksInDBContent();

            //Act
            BookRepository bookRepo = new BookRepository(_booksDBContext, _mockIsbnChecker.Object);
            var book = await bookRepo.GetBookAsync(9781401309619);

            book.Title = "Updated Title";

            await bookRepo.UpdateBookAsync(book);

            var bookAfterUpdate = await bookRepo.GetBookAsync(9781401309619);

            //Assert
            Assert.IsNotNull(bookAfterUpdate);
            Assert.IsTrue(bookAfterUpdate.Title == "Updated Title");


        }

        /// <summary>
        /// UpdateBook -  book to update not found
        /// </summary>
        [TestMethod]
        public async Task UpdateBookAsync_NotFoundFailAsync()
        {
            //Arrange
            _mockIsbnChecker = new Mock<IIsbnChecker>();
            _mockIsbnChecker.Setup(m => m.isValidISBNCode(It.IsAny<String>())).Returns(true);
            _booksDBContext = _bookRepositoryTestHelper.PopulateAllBooksInDBContent();

            //Act
            BookRepository bookRepo = new BookRepository(_booksDBContext, _mockIsbnChecker.Object);

            var book = await bookRepo.GetBookAsync(9781401309619);

            book.Title = "Updated Title";
            book.ISBN = 123999;

            try
            {
                await bookRepo.UpdateBookAsync(book);

            }
            catch (NotFoundException exception)
            {
                //Assert
                Assert.IsTrue(exception.Message == "No Book Found to update");
            }

        }


        /// <summary>
        /// AddBook -  success
        /// </summary>
        [TestMethod]
        public async Task AddBookAsync_SuccessAsync()
        {
            //Arrange
            _mockIsbnChecker = new Mock<IIsbnChecker>();
            _mockIsbnChecker.Setup(m => m.isValidISBNCode(It.IsAny<String>())).Returns(true);
            _booksDBContext = _bookRepositoryTestHelper.PopulateNoBooksInDBContent();


            //Act
            BookRepository bookRepo = new BookRepository(_booksDBContext, _mockIsbnChecker.Object);

            var bookToBeAdded = new Book()
            {
                ISBN = 9780671612979,
                Title = "Ben Hogan's Five Lessons: The Modern Fundamentals of Golf",
                Authors = "Ben Hogan; Herbert Warren Wind",
                Publisher = "Touchstone"

            };

            await bookRepo.AddBookAsync(bookToBeAdded);

            var book = await bookRepo.GetBookAsync(9780671612979);

            //Assert
            Assert.IsNotNull(book);
            Assert.IsTrue(book.ISBN == 9780671612979);
            Assert.IsTrue(book.Title == "Ben Hogan's Five Lessons: The Modern Fundamentals of Golf");
            Assert.IsTrue(book.Authors == "Ben Hogan; Herbert Warren Wind");
            Assert.IsTrue(book.Publisher == "Touchstone");

        }

        /// <summary>
        /// AddBook - try to add book which already exists
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task AddBookAsync_FailAlreadyExisitsAsync()
        {
            //Arrange
            _mockIsbnChecker = new Mock<IIsbnChecker>();
            _mockIsbnChecker.Setup(m => m.isValidISBNCode(It.IsAny<String>())).Returns(true);
            _booksDBContext = _bookRepositoryTestHelper.PopulateAllBooksInDBContent();


            //Act
            BookRepository bookRepo = new BookRepository(_booksDBContext, _mockIsbnChecker.Object);

            var bookToBeAdded = new Book()
            {
                ISBN = 9780671612979,
                Title = "Ben Hogan's Five Lessons: The Modern Fundamentals of Golf",
                Authors = "Ben Hogan; Herbert Warren Wind",
                Publisher = "Touchstone"

            };

            try
            {
                await bookRepo.AddBookAsync(bookToBeAdded);

            }
            catch (BookAlreadyExistsException exception)
            {
                //Assert
                Assert.IsTrue(exception.Message == "Book Already Exists");
            }



        }

        /// <summary>
        /// AddBook - try to add book with incorrectly formatted isbn number
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task AddBookAsync_FailIsbnAsync()
        {
            //Arrange
            _mockIsbnChecker = new Mock<IIsbnChecker>();

            //make sure the check fails
            _mockIsbnChecker.Setup(m => m.isValidISBNCode(It.IsAny<String>())).Returns(false);

            _booksDBContext = _bookRepositoryTestHelper.PopulateNoBooksInDBContent();


            //Act
            BookRepository bookRepo = new BookRepository(_booksDBContext, _mockIsbnChecker.Object);

            var bookToBeAdded = new Book()
            {
                ISBN = 123,
                Title = "Ben Hogan's Five Lessons: The Modern Fundamentals of Golf",
                Authors = "Ben Hogan; Herbert Warren Wind",
                Publisher = "Touchstone"

            };

            try
            {
                await bookRepo.AddBookAsync(bookToBeAdded);

            }
            catch (NotValidIsbnException exception)
            {
                //Assert
                Assert.IsTrue(exception.Message == "Not Valid ISBN");
            }



        }

        /// <summary>
        /// Delete Book - Success
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task DeleteBookAsync_SuccessAsync()
        {
            //Arrange
            _mockIsbnChecker = new Mock<IIsbnChecker>();
            _mockIsbnChecker.Setup(m => m.isValidISBNCode(It.IsAny<String>())).Returns(true);
            _booksDBContext = _bookRepositoryTestHelper.PopulateAllBooksInDBContent();

            //Act
            BookRepository bookRepo = new BookRepository(_booksDBContext, _mockIsbnChecker.Object);

            await bookRepo.DeleteBookAsync(9780671612979);

            var books = await bookRepo.GetBooksAsync();

            //Asert
            Assert.IsTrue(books.Count == 1);

        }

        /// <summary>
        /// Delete Book - try to delete book which doesnt exist
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task DeleteBookAsync_FailNoBookAsync()
        {
            //Arrange
            _mockIsbnChecker = new Mock<IIsbnChecker>();
            _mockIsbnChecker.Setup(m => m.isValidISBNCode(It.IsAny<String>())).Returns(true);
            _booksDBContext = _bookRepositoryTestHelper.PopulateAllBooksInDBContent();

            //Act
            BookRepository bookRepo = new BookRepository(_booksDBContext, _mockIsbnChecker.Object);

            try
            {
                await bookRepo.DeleteBookAsync(123);

            }
            catch (NotFoundException exception)
            {
                //Assert
                Assert.IsTrue(exception.Message == "No Book Found to delete");
            }


        }



    }
}