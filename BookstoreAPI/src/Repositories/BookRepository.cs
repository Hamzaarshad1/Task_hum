using BookstoreAPI.Configuration;
using BookstoreAPI.Models;
using MongoDB.Driver;

namespace BookstoreAPI.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IMongoCollection<Book> _books;

        public BookRepository(IMongoClient client, IMongoDbSettings settings)
        {
            var database = client.GetDatabase(settings.DatabaseName);
            _books = database.GetCollection<Book>(settings.BooksCollectionName);
        }

        public async Task<List<Book>> GetAllAsync() =>
            await _books.Find(_ => true).ToListAsync();

        public async Task<Book?> GetByIdAsync(string id) =>
            await _books.Find(b => b.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Book book) =>
            await _books.InsertOneAsync(book);

        public async Task UpdateAsync(string id, Book book) =>
            await _books.ReplaceOneAsync(b => b.Id == id, book);

        public async Task DeleteAsync(string id) =>
            await _books.DeleteOneAsync(b => b.Id == id);
    }
}

