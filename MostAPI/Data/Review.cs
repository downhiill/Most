using MongoDB.Bson;

namespace MostAPI.Data
{
    public class Review
    {
        public int Id { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public string Username { get; set; } 

        // Это поле должно быть массивом байтов для загрузки файла
        public byte[] Photo { get; set; }
    }
}
