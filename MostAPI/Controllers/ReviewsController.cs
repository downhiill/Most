using Microsoft.AspNetCore.Mvc;

namespace MostAPI.Controllers
{
    public class ReviewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
