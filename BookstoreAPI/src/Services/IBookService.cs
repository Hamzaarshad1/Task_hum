using BookstoreAPI.Models;

namespace BookstoreAPI.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(string id);
        Task CreateBookAsync(Book book);
        Task UpdateBookAsync(string id, Book book);
        Task DeleteBookAsync(string id);
    }
}

