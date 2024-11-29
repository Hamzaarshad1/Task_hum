using BookstoreAPI.Controllers;
using BookstoreAPI.Models;
using BookstoreAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

public class BooksControllerTests
{
    private readonly Mock<IBookService> _bookServiceMock;
    private readonly Mock<ILogger<BooksController>> _loggerMock;
    private readonly BooksController _controller;

    public BooksControllerTests()
    {
        _bookServiceMock = new Mock<IBookService>();
        _loggerMock = new Mock<ILogger<BooksController>>();
        _controller = new BooksController(_bookServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllBooks_ReturnsOkResult_WithListOfBooks()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book { Id = "1", Title = "Sample Book 1" },
            new Book { Id = "2", Title = "Sample Book 2" }
        };

        _bookServiceMock.Setup(service => service.GetAllBooksAsync())
            .ReturnsAsync(books);

        // Act
        var result = await _controller.GetAllBooks();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedBooks = Assert.IsType<List<Book>>(okResult.Value);
        Assert.Equal(2, returnedBooks.Count);
    }

    [Fact]
    public async Task GetAllBooks_ReturnsInternalServerError_OnException()
    {
        // Arrange
        _bookServiceMock.Setup(service => service.GetAllBooksAsync())
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.GetAllBooks();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task GetBook_ReturnsOkResult_WithBook()
    {
        // Arrange
        var book = new Book { Id = "1", Title = "Sample Book" };

        _bookServiceMock.Setup(service => service.GetBookByIdAsync("1"))
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
        _bookServiceMock.Setup(service => service.GetBookByIdAsync("1"))
            .ReturnsAsync((Book)null);

        // Act
        var result = await _controller.GetBook("1");

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateBook_ReturnsCreatedAtActionResult_WithCreatedBook()
    {
        // Arrange
        var book = new Book { Id = "1", Title = "New Book" };

        _bookServiceMock.Setup(service => service.CreateBookAsync(book))
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

        _bookServiceMock.Setup(service => service.CreateBookAsync(book))
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

        _bookServiceMock.Setup(service => service.GetBookByIdAsync("1"))
            .ReturnsAsync(existingBook);
        _bookServiceMock.Setup(service => service.UpdateBookAsync("1", updatedBook))
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

        _bookServiceMock.Setup(service => service.GetBookByIdAsync("1"))
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

        _bookServiceMock.Setup(service => service.GetBookByIdAsync("1"))
            .ReturnsAsync(existingBook);
        _bookServiceMock.Setup(service => service.DeleteBookAsync("1"))
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
        _bookServiceMock.Setup(service => service.GetBookByIdAsync("1"))
            .ReturnsAsync((Book)null);

        // Act
        var result = await _controller.DeleteBook("1");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
