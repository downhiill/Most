namespace MostAPI.Data
{
    public class Services
    {
        public int Id { get; set; } // MongoDB использует строковые идентификаторы
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
    }
}
