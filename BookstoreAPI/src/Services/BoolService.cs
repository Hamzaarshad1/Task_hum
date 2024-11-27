using BookstoreAPI.Models;
using BookstoreAPI.Repositories;

namespace BookstoreAPI.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<Book>> GetAllBooksAsync() =>
            await _bookRepository.GetAllAsync();

        public async Task<Book?> GetBookByIdAsync(string id) =>
            await _bookRepository.GetByIdAsync(id);

        public async Task CreateBookAsync(Book book) =>
            await _bookRepository.CreateAsync(book);

        public async Task UpdateBookAsync(string id, Book book) =>
            await _bookRepository.UpdateAsync(id, book);

        public async Task DeleteBookAsync(string id) =>
            await _bookRepository.DeleteAsync(id);
    }
}

