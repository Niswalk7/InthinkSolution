using Microsoft.AspNetCore.Mvc;

namespace InthinkSolution.Controllers
{
    public class SupervisorController : Controller
    {
        [HttpGet]
        public IActionResult SDashboard()
        {
            if (HttpContext.Session.GetString("Role") == "Supervisor")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
