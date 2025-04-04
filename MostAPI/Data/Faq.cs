﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MostAPI.Data
{
    public class Faq
    {
        public int Id { get; set; } // ID записи, MongoDB будет автоматически генерировать этот ID
        public string Question { get; set; } // Вопрос
        public string Answer { get; set; } // Ответ
    }

}
