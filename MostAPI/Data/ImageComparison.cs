using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MostAPI.Data
{
    public class ImageComparison
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public byte[] Image1 { get; set; } // Данные первого изображения
        public byte[] Image2 { get; set; } // Данные второго изображения
    }
}
