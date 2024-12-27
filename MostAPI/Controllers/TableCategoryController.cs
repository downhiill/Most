using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MostAPI.Context;
using MostAPI.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MostAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableCategoryController : ControllerBase
    {
        private readonly MongoDbContext _mongoContext;

        public TableCategoryController(MongoDbContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        // GET: api/tablecategory/shoes
        [HttpGet("shoes")]
        public async Task<ActionResult<IEnumerable<Shoes>>> GetShoes()
        {
            var shoes = await _mongoContext.Shoes.Find(_ => true).ToListAsync();
            return Ok(shoes);
        }

        // GET: api/tablecategory/bag
        [HttpGet("bag")]
        public async Task<ActionResult<IEnumerable<Bag>>> GetBag()
        {
            var bags = await _mongoContext.Bags.Find(_ => true).ToListAsync();
            return Ok(bags);
        }

        // GET: api/tablecategory/drycleaning
        [HttpGet("drycleaning")]
        public async Task<ActionResult<IEnumerable<DryCleaning>>> GetDryCleaning()
        {
            var dryCleanings = await _mongoContext.DryCleanings.Find(_ => true).ToListAsync();
            return Ok(dryCleanings);
        }

        // GET: api/tablecategory/delivery
        [HttpGet("delivery")]
        public async Task<ActionResult<IEnumerable<Delivery>>> GetDelivery()
        {
            var deliveries = await _mongoContext.Deliveries.Find(_ => true).ToListAsync();
            return Ok(deliveries);
        }
    }
}
