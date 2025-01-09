using Microsoft.AspNetCore.Mvc;
using InthinkSolution.Models;
using System.Net.Mail;
using System.Net;

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
        new User { Username = "admin", Email = "nischalc007@gmail.com" }
    };

            var user = users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                ModelState.AddModelError("", "Username not found.");
                return View();
            }

            // Generate a unique token and link
            var resetToken = Guid.NewGuid().ToString();
            var resetLink = Url.Action("ResetPassword", "Login", new { token = resetToken }, Request.Scheme);

            // Send email
            SendPasswordResetEmail(user.Email, resetLink);

            ViewBag.Message = "Password reset instructions have been sent to your registered email.";
            return View();
        }

        private void SendEmail(string toEmail, string subject, string body)
        {
            // Simulate email sending logic
            Console.WriteLine($"To: {toEmail}\nSubject: {subject}\nBody: {body}");
        }

        public void SendPasswordResetEmail(string recipientEmail, string resetLink)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("nischalc007@gmail.com", "ebbs fmmu zfms ianu"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("your-email@gmail.com"),
                Subject = "Password Reset Request",
                Body = $"<p>Hello,</p><p>Click the link below to reset your password:</p><p><a href='{resetLink}'>Reset Password</a></p><p>If you did not request this, please ignore this email.</p>",
                IsBodyHtml = true,
            };

            mailMessage.To.Add(recipientEmail);

            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            // Validate token (add token storage & validation logic)
            return View(new ResetPasswordViewModel { Token = token });
        }

        [HttpPost]
        public IActionResult ResetPassword(string username, string newPassword)
        {
            // Example logic to reset the password
            if (UpdatePasswordInDatabase(username, newPassword))
            {
                // Set a success message for the login page
                TempData["Message"] = "Your password has been reset successfully. Please log in.";

                // Redirect to the login page
                return RedirectToAction("Login", "Login");
            }
            else
            {
                ModelState.AddModelError("", "An error occurred while resetting your password.");
                return View();
            }
        }

        // Simulated method to update the password in the database
        private bool UpdatePasswordInDatabase(string username, string newPassword)
        {
            // Add your logic to update the password in the database here
            return true; // Assume success for demonstration purposes
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
