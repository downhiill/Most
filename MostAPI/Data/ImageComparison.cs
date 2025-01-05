using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MostAPI.Data
{
    public class ImageComparison
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // Уникальный идентификатор сравнения
        public byte[] Image1 { get; set; } // Данные первого изображения
        public byte[] Image2 { get; set; } // Данные второго изображения
    }
}
