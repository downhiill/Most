namespace MostAPI.Data
{
    public class ImageComparisonRequest
    {
        public IFormFile Image1 { get; set; } // Первое изображение
        public IFormFile Image2 { get; set; } // Второе изображение
    }
}
