using System.ComponentModel.DataAnnotations;

namespace MostAPI.Data
{
    public class ImageComparisonRequest
    {
        [Required(ErrorMessage = "Первое изображение обязательно")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "Размер файла не должен превышать 5 МБ.")]
        public IFormFile Image1 { get; set; } // Первое изображение

        [Required(ErrorMessage = "Второе изображение обязательно.")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "Размер файла не должен превышать 5 МБ.")]
        public IFormFile Image2 { get; set; } // Второе изображение
    }

    // Кастомный атрибут для проверки размера файла
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxSize;

        public MaxFileSizeAttribute(int maxSize)
        {
            _maxSize = maxSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file && file.Length > _maxSize)
            {
                return new ValidationResult(ErrorMessage ?? $"Размер файла не должен превышать {_maxSize / (1024 * 1024)} МБ.");
            }

            return ValidationResult.Success;
        }
    }
}
