using BookstoreAPI.Models;
using BookstoreAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookService bookService, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetAllBooks()
        {
            try
            {
                var books = await _bookService.GetAllBooksAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all books");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(string id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                {
                    return NotFound();
                }
                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting book with id {Id}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook(Book book)
        {
            try
            {
                await _bookService.CreateBookAsync(book);
                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new book");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(string id, Book book)
        {
            try
            {
                var existingBook = await _bookService.GetBookByIdAsync(id);
                if (existingBook == null)
                {
                    return NotFound();
                }
                await _bookService.UpdateBookAsync(id, book);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating book with id {Id}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                {
                    return NotFound();
                }
                await _bookService.DeleteBookAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting book with id {Id}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}

