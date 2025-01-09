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
                    // Add an error message for invalid credentials
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            return View(model); // Return to the login view with validation errors
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string username)
        {
            // Simulate fetching user details from the database
            var users = new List<User>
    {
        new User { Username = "admin", Email = "admin@example.com" },
        new User { Username = "manufacturer", Email = "manufacturer@example.com" }
    };

            var user = users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                ModelState.AddModelError("", "Username not found.");
                return View();
            }

            // Simulate sending a password reset email
            var resetLink = $"https://www.example.com/Login/ResetPassword?token={Guid.NewGuid()}";
            SendEmail(user.Email, "Password Reset Request", $"Click the link to reset your password: {resetLink}");

            ViewBag.Message = "Password reset instructions have been sent to your registered email.";
            return View();
        }

        private void SendEmail(string toEmail, string subject, string body)
        {
            // Simulate email sending logic
            Console.WriteLine($"To: {toEmail}\nSubject: {subject}\nBody: {body}");
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
