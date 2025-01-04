using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace MostAPI.Data
{
    public class Review
    {
        public ObjectId Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        public byte[] Photo { get; set; }
    }

}
