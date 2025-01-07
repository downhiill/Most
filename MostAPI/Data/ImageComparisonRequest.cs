using System.ComponentModel.DataAnnotations;

namespace MostAPI.Data
{
    public class ImageComparisonRequest
    {
        public IFormFile? Image1 { get; set; } // Первое изображение (может быть null)
        public IFormFile? Image2 { get; set; } // Второе изображение (может быть null)
    }
}
