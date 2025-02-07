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
            return View(model);
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                bool isValidUser = ValidateUser(model.Username, model.Password, model.Role);

                if (isValidUser)
                {
               
                    HttpContext.Session.SetString("Username", model.Username);
                    HttpContext.Session.SetString("Role", model.Role);

                   
                    return model.Role switch
                    {
                        "Admin" => RedirectToAction("Dashboard", "Admin"),
                        "Manufacturer" => RedirectToAction("MDashboard", "Manufacturer"),
                        "Supervisor" => RedirectToAction("SDashboard", "Supervisor"),
                        "Operator" => RedirectToAction("ODashboard", "Operator"),
                        _ => RedirectToAction("Login"), 
                    };
                }
                else
                {
                 
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            return View(model); 
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string username)
        {
            
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

            
            var resetToken = Guid.NewGuid().ToString();
            var resetLink = Url.Action("ResetPassword", "Login", new { token = resetToken }, Request.Scheme);

           
            SendPasswordResetEmail(user.Email, resetLink);

            ViewBag.Message = "Password reset instructions have been sent to your registered email.";
            return View();
        }

        private void SendEmail(string toEmail, string subject, string body)
        {
           
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
            
            return View(new ResetPasswordViewModel { Token = token });
        }

        [HttpPost]
        public IActionResult ResetPassword(string username, string newPassword)
        {
           
            if (UpdatePasswordInDatabase(username, newPassword))
            {
                
                TempData["Message"] = "Your password has been reset successfully. Please log in.";

                
                return RedirectToAction("Login", "Login");
            }
            else
            {
                ModelState.AddModelError("", "An error occurred while resetting your password.");
                return View();
            }
        }

        
        private bool UpdatePasswordInDatabase(string username, string newPassword)
        {
            
            return true; 
        }




        private bool ValidateUser(string username, string password, string role)
        {
            
            var users = new List<LoginViewModel>
            {
                new LoginViewModel { Username = "admin", Password = "admin123", Role = "Admin" },
                new LoginViewModel { Username = "manufacturer", Password = "manu123", Role = "Manufacturer" },
                new LoginViewModel { Username = "supervisor", Password = "super123", Role = "Supervisor" },
                new LoginViewModel { Username = "operator", Password = "oper123", Role = "Operator" }
            };

            
            return users.Any(u => u.Username == username && u.Password == password && u.Role == role);
        }
    }
}
