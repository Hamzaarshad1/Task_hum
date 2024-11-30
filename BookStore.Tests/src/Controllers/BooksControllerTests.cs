using BookstoreAPI.Controllers;
using BookstoreAPI.Models;
using BookstoreAPI.Repositories;
using BookstoreAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

public class BooksControllerTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly Mock<ILogger<BooksController>> _loggerMock;
    private readonly IBookService _bookService;
    private readonly BooksController _controller;

    public BooksControllerTests()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _bookService = new BookService(_bookRepositoryMock.Object);
        _loggerMock = new Mock<ILogger<BooksController>>();
        _controller = new BooksController(_bookService, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllBooks_ReturnsOkResultWithBooks()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book { Id = "1", Title = "Sample Book 1" },
            new Book { Id = "2", Title = "Sample Book 2" }
        };
        _bookRepositoryMock.Setup(repository => repository.GetAllBooksAsync(1, 5)).ReturnsAsync(books);
         _bookRepositoryMock.Setup(repository => repository.GetTotalBooksAsync()).ReturnsAsync(1);

        // Act
        var result = await _controller.GetAllBooks(1, 5);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var value = okResult.Value;

        // Check anonymous object properties
        Assert.NotNull(value);
        Assert.Equal(2, value.GetType().GetProperties().Length);

        // Check books property
        var booksProperty = value.GetType().GetProperty("books");
        Assert.NotNull(booksProperty);
        Assert.Equal(typeof(List<Book>), booksProperty.PropertyType);
        Assert.Equal(books, booksProperty.GetValue(value));

        // Check totalBooks property
        var totalBooksProperty = value.GetType().GetProperty("totalBooks");
        Assert.NotNull(totalBooksProperty);
        Assert.Equal(typeof(int), totalBooksProperty.PropertyType);
        Assert.Equal(1, totalBooksProperty.GetValue(value));
    }

    [Fact]
    public async Task GetAllBooks_ReturnsBadRequest_OnInvalidPageNumber()
    {
        // Act
        var result = await _controller.GetAllBooks(0, 1);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);

    }

    [Fact]
    public async Task GetAllBooks_ReturnsBadRequest_OnInvalidPageSize()
    {
        // Act
        var result = await _controller.GetAllBooks(1, 0);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }


    [Fact]
    public async Task GetAllBooks_ReturnsInternalServerError_OnException()
    {
        // Arrange
        _bookRepositoryMock.Setup(repository => repository.GetAllBooksAsync(1,5))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.GetAllBooks();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);

        Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        
        // logger should be called once with LogLevel.Error
        _loggerMock.Verify(
            logger => logger.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }

    [Fact]
    public async Task GetBook_ReturnsOkResult_WithBook()
    {
        // Arrange
        var book = new Book { Id = "1", Title = "Sample Book" };

        _bookRepositoryMock.Setup(repository => repository.GetBookByIdAsync("1"))
            .ReturnsAsync(book);

        // Act
        var result = await _controller.GetBook("1");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedBook = Assert.IsType<Book>(okResult.Value);
        Assert.Equal("1", returnedBook.Id);
    }

    [Fact]
    public async Task GetBook_ReturnsNotFound_WhenBookDoesNotExist()
    {
        // Arrange
        _bookRepositoryMock.Setup(repository => repository.GetBookByIdAsync("1"))
            .ReturnsAsync((Book)null);

        // Act
        var result = await _controller.GetBook("1");

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetBook_ReturnsInternalServerError_OnException()
    {
        // Arrange
        _bookRepositoryMock.Setup(repository => repository.GetBookByIdAsync("1"))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.GetBook("1");

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
    }

    [Fact]
    public async Task CreateBook_ReturnsCreatedAtActionResult_WithCreatedBook()
    {
        // Arrange
        var book = new Book { Id = "1", Title = "New Book" };

        _bookRepositoryMock.Setup(repository => repository.CreateBookAsync(book))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CreateBook(book);

        // Assert
        var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedBook = Assert.IsType<Book>(createdAtResult.Value);
        Assert.Equal("1", returnedBook.Id);
    }

    [Fact]
    public async Task CreateBook_ReturnsInternalServerError_OnException()
    {
        // Arrange
        var book = new Book { Title = "New Book" };

        _bookRepositoryMock.Setup(repository => repository.CreateBookAsync(book))
            .ThrowsAsync(new Exception("Error saving to database"));

        // Act
        var result = await _controller.CreateBook(book);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task UpdateBook_ReturnsNoContentResult_WhenUpdateIsSuccessful()
    {
        // Arrange
        var existingBook = new Book { Id = "1", Title = "Existing Book" };
        var updatedBook = new Book { Title = "Updated Book" };

        _bookRepositoryMock.Setup(repository=> repository.GetBookByIdAsync("1"))
            .ReturnsAsync(existingBook);
        _bookRepositoryMock.Setup(repository => repository.UpdatBookAsync("1", updatedBook))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateBook("1", updatedBook);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateBook_ReturnsNotFound_WhenBookDoesNotExist()
    {
        // Arrange
        var updatedBook = new Book { Title = "Updated Book" };

        _bookRepositoryMock.Setup(repository => repository.GetBookByIdAsync("1"))
            .ReturnsAsync((Book)null);

        // Act
        var result = await _controller.UpdateBook("1", updatedBook);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteBook_ReturnsNoContentResult_WhenDeleteIsSuccessful()
    {
        // Arrange
        var existingBook = new Book { Id = "1", Title = "Book to Delete" };

        _bookRepositoryMock.Setup(repository => repository.GetBookByIdAsync("1"))
            .ReturnsAsync(existingBook);
        _bookRepositoryMock.Setup(repository => repository.DeleteBookAsync("1"))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteBook("1");

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteBook_ReturnsNotFound_WhenBookDoesNotExist()
    {
        // Arrange
        _bookRepositoryMock.Setup(repository => repository.GetBookByIdAsync("1"))
            .ReturnsAsync((Book)null);

        // Act
        var result = await _controller.DeleteBook("1");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteBook_ReturnsInternalServerError_OnException()
    {
        // Arrange
        _bookRepositoryMock.Setup(repository => repository.GetBookByIdAsync("1"))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.DeleteBook("1");

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }
}
