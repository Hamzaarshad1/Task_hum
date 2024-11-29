using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BookstoreAPI.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required]
        [BsonElement("title")]
        public string Title { get; set; } = null!;

        [BsonElement("author")]
        public string Author { get; set; } = null!;

        [BsonElement("price")]
        public decimal Price { get; set; }
    }
}

