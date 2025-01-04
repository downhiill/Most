using System.ComponentModel.DataAnnotations;

namespace MostAPI.Data
{
    public class ReviewRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        public IFormFile UploadedPhoto { get; set; }
    }
}
