using System.ComponentModel.DataAnnotations;

namespace MostAPI.Data
{
   

    public class ImageComparisonRequest
    {
        [Required(ErrorMessage = "Первое изображение обязательно.")]
        public IFormFile Image1 { get; set; } // Первое изображение

        [Required(ErrorMessage = "Второе изображение обязательно.")]
        public IFormFile Image2 { get; set; } // Второе изображение
    }

}
