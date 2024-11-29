using BookstoreAPI.Configuration;
using BookstoreAPI.Models;
using MongoDB.Driver;
using MongoDB.Bson;

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

        public async Task<List<Book>> GetAllBooksAsync(int pageNumber, int pageSize) {
            return await _books.Find(b => true)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(string id) =>
            await _books.Find(b => b.Id == id).FirstOrDefaultAsync();

        public async Task CreateBookAsync(Book book) =>
            await _books.InsertOneAsync(book);

        public async Task UpdatBookAsync(string id, Book book) =>
            await _books.ReplaceOneAsync(b => b.Id == id, book);

        public async Task DeleteBookAsync(string id) =>
            await _books.DeleteOneAsync(b => b.Id == id);

        public async Task<int> GetTotalBooksAsync()
        {
            long count = await _books.CountDocumentsAsync(new BsonDocument());
            
            if (count > int.MaxValue)
            {
                throw new InvalidOperationException("The total number of documents exceeds the maximum value of int.");
            }

            return (int)count;
        }
    }
}

