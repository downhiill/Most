namespace MostAPI.Data
{
    public class Review
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public List<string> Images { get; set; } = new List<string>();
    }
}
