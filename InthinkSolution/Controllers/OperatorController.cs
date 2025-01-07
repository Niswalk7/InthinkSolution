using Microsoft.AspNetCore.Mvc;

namespace InthinkSolution.Controllers
{
    public class OperatorController : Controller
    {
        [HttpGet]
        public IActionResult ODashboard()
        {
            
                return View();
          
            
        }
    }
}
