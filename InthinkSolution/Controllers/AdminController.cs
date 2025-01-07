using Microsoft.AspNetCore.Mvc;
using InthinkSolution.Models;

namespace InthinkSolution.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Index", "Login"); // Redirect to login if not authorized
            }

            return View();
        }

        // GET: Admin/CreateUser
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        // GET: Admin/ManageUsers
        [HttpGet]
        public IActionResult ManageUsers()
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]
        public IActionResult CreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Add logic to save the new user to the database
                return RedirectToAction("ManageUsers");
            }
            return View(model);
        }

        // GET: Admin/EditUser
        [HttpGet]
        public IActionResult EditUser(int id)
        {
            // Fetch user data by ID for editing
            return View();
        }

        // POST: Admin/EditUser
        [HttpPost]
        public IActionResult EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Add logic to update user data in the database
                return RedirectToAction("ManageUsers");
            }
            return View(model);
        }

        // GET: Admin/DeleteUser
        [HttpGet]
        public IActionResult DeleteUser(int id)
        {
            // Add logic to delete the user by ID
            return RedirectToAction("ManageUsers");
        }
    }

   
}
