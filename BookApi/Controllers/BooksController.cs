using Books.Exceptions;
using Books.Models;
using Books.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Books.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {

        private IBookRepository _bookRepository;

        /// <summary>
        /// Provides an API which uses the bookRepository.
        /// </summary>
        /// <param name="bookRepository"></param>
        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }


        /// <summary>
        /// Gets all the books in the library
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> IndexAsync()
        {

            List<Book> books;

            try
            {
                books = await _bookRepository.GetBooksAsync();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return Ok(books);
        }

        /// <summary>
        /// Adds a Book to the library
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> AddAsync(Book book)
        {
            try
            {
                await _bookRepository.AddBookAsync(book);
            }
            catch (BookAlreadyExistsException)
            {
                return Conflict();
            }
            catch (NotValidIsbnException)
            {
                return BadRequest();
            }

            return Created();
        }


        /// <summary>
        /// Get one book from the library
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns></returns>
        [HttpGet("{isbn}")]
        public async Task<IActionResult> GetBookAsync(long isbn)
        {
            Book book;
            try
            {
                book = await _bookRepository.GetBookAsync(isbn);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            return Ok(book);
        }


        /// <summary>
        /// Update a book in the library
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPut()]
        public async Task<IActionResult> EditAsync(Book book)
        {
            try
            {
                await _bookRepository.UpdateBookAsync(book);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception exception)
            {
                return NotFound();
            }
            return Ok(book);
        }


        /// <summary>
        /// Delete a book from the library
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns></returns>
        [HttpDelete("{isbn}")]
        public async Task<IActionResult> DeleteAsync(long isbn)
        {
            try
            {
                await _bookRepository.DeleteBookAsync(isbn);
            }
            catch (NotFoundException)
            {
                return NotFound();

            }
            return Ok();
        }
    }
}
