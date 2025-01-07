using Microsoft.AspNetCore.Mvc;
using InthinkSolution.Models;

namespace InthinkSolution.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginViewModel();
            return View(model); // Pass the model to the view
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Validate user credentials
                bool isValidUser = ValidateUser(model.Username, model.Password, model.Role);

                if (isValidUser)
                {
                    // Set session variables for the logged-in user
                    HttpContext.Session.SetString("Username", model.Username);
                    HttpContext.Session.SetString("Role", model.Role);

                    // Redirect to dashboards based on the user's role
                    return model.Role switch
                    {
                        "Admin" => RedirectToAction("Dashboard", "Admin"),
                        "Manufacturer" => RedirectToAction("MDashboard", "Manufacturer"),
                        "Supervisor" => RedirectToAction("SDashboard", "Supervisor"),
                        "Operator" => RedirectToAction("ODashboard", "Operator"),
                        _ => RedirectToAction("Login"), // Redirect back to login if role is invalid
                    };
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username, password, or role.");
                }
            }

            return View(model); // Return to the login view with validation errors
        }

        private bool ValidateUser(string username, string password, string role)
        {
            // Simulate database logic with hardcoded user data
            var users = new List<LoginViewModel>
            {
                new LoginViewModel { Username = "admin", Password = "admin123", Role = "Admin" },
                new LoginViewModel { Username = "manufacturer", Password = "manu123", Role = "Manufacturer" },
                new LoginViewModel { Username = "supervisor", Password = "super123", Role = "Supervisor" },
                new LoginViewModel { Username = "operator", Password = "oper123", Role = "Operator" }
            };

            // Check if the user exists with the given credentials
            return users.Any(u => u.Username == username && u.Password == password && u.Role == role);
        }
    }
}
