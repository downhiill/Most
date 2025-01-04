using MongoDB.Bson;

namespace MostAPI.Data
{
    public class Review
    {
        public ObjectId Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public byte[] Photo { get; set; } // Фото в бинарном формате
        public string Text { get; set; }
    }
}
