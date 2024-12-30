using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MostAPI.Data
{
    public class Faq
    {
        public string Id { get; set; } // ID записи
        public string Question { get; set; } // Вопрос
        public string Answer { get; set; } // Ответ
    }

}
