namespace MostAPI.Data
{

    public class Category
    {
        public int Id { get; set; } // MongoDB идентификатор
        public string Name { get; set; }
        public List<Services> Services { get; set; }
    }
}
