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
        [BsonElement("Title")]
        public string Title { get; set; } = null!;

        [BsonElement("Author")]
        public string Author { get; set; } = null!;

        [BsonElement("Price")]
        public decimal Price { get; set; }
    }
}

