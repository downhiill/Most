namespace MostAPI.Data
{

    public class Category
    {
        public string Id { get; set; } // MongoDB идентификатор
        public string Name { get; set; }
        public List<Services> Services { get; set; }
    }
}
