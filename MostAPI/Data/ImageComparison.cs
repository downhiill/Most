namespace MostAPI.Data
{
    public class ImageComparison
    {
        public string Id { get; set; } // Уникальный идентификатор сравнения
        public byte[] Image1 { get; set; } // Данные первого изображения
        public byte[] Image2 { get; set; } // Данные второго изображения
        public string Description { get; set; } // Дополнительное описание
    }
}
