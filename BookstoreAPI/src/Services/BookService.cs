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

        public async Task<List<Book>> GetAllBooksAsync(int pageNumber, int pageSize){
            var books = await _bookRepository.GetAllBooksAsync(pageNumber, pageSize);
            return books;
        }

        public async Task<Book?> GetBookByIdAsync(string id){
            if (string.IsNullOrEmpty(id)){
                return null;
            }
            return await _bookRepository.GetBookByIdAsync(id);
        }

        public async Task CreateBookAsync(Book book) {
            if (book == null){
                throw new ArgumentNullException(nameof(book));
            }
            await _bookRepository.CreateBookAsync(book);
        }

        public async Task UpdateBookAsync(string id, Book book) {
            if (string.IsNullOrEmpty(id) || book == null){
                throw new ArgumentNullException();
            }
            await _bookRepository.UpdatBookAsync(id, book);
        }

        public async Task DeleteBookAsync(string id) {
            if (string.IsNullOrEmpty(id)){
                throw new ArgumentNullException(nameof(id));
            }
            await _bookRepository.DeleteBookAsync(id);
        }

        public async Task<int> GetTotalBooksAsync(){
            return await _bookRepository.GetTotalBooksAsync();
        }
    }
}

