using BookstoreAPI.Models;

namespace BookstoreAPI.Repositories
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllBooksAsync(int pageNumber, int pageSize);
        Task<Book?> GetBookByIdAsync(string id);
        Task CreateBookAsync(Book book);
        Task UpdatBookAsync(string id, Book book);
        Task DeleteBookAsync(string id);
        Task<int> GetTotalBooksAsync();
    }
}

