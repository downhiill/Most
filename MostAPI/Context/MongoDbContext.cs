using MongoDB.Driver;
using MostAPI.Data;

namespace MostAPI.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IMongoClient mongoClient)
        {
        }

        public IMongoCollection<Shoes> Shoes => _database.GetCollection<Shoes>("Shoes");
        public IMongoCollection<Bag> Bags => _database.GetCollection<Bag>("Bags");
        public IMongoCollection<DryCleaning> DryCleanings => _database.GetCollection<DryCleaning>("DryCleanings");
        public IMongoCollection<Delivery> Deliveries => _database.GetCollection<Delivery>("Deliveries");
    }
}
