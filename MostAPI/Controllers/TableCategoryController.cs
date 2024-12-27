using Microsoft.AspNetCore.Mvc;
using MostAPI.Data;

namespace MostAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableCategoryController : Controller
    {
        private readonly ApplicationDbContext _context; 

        public TableCategoryController(ApplicationDbContext context) { _context = context; } 
        
        // GET api/shoes
        [HttpGet] 
        public async Task<ActionResult<IEnumerable<Shoes>>> GetShoes() 
        { 
            return await _context.Shoes.ToListAsync(); 
        }

        // GET api/bag
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bag>>> GetBag()
        {
            return await _context.Bag.ToListAsync();
        }

        // GET api/drycleaning
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DryCleaning>>> GetDryCleaning()
        {
            return await _context.DryCleaning.ToListAsync();
        }

        // GET api/drycleaning
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Delivery>>> GetDelivery()
        {
            return await _context.Delivery.ToListAsync();
        }


    }
}
