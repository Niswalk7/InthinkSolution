using Microsoft.AspNetCore.Mvc;

namespace InthinkSolution.Controllers
{
    public class ManufacturerController : Controller
    {
        [HttpGet]
        public IActionResult MDashboard()
        {
            
                return View();
           
        }
    }
}
