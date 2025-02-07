using InthinkSolution.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;




public class AdminController : Controller
{
    private readonly AppDbContext _context;



    private readonly string _connectionString;

    
    public AdminController(IConfiguration configuration,AppDbContext context)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _context = context;
    }
    public IActionResult Dashboard()
    {
        

        // Fetch all users
        var userList = _context.Users.ToList();
        
            return View(userList);
    }

    public IActionResult CreateUser()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateUser(CreateUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Users (Username,Email, Password, Role, FirstName, LastName) VALUES (@Username,@Email, @Password, @Role, @FirstName, @LastName)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", model.Username);
                    cmd.Parameters.AddWithValue("@Email", model.Email);
                    cmd.Parameters.AddWithValue("@Password", model.Password); // Hash this in real-world apps
                    cmd.Parameters.AddWithValue("@Role", model.Role);
                    cmd.Parameters.AddWithValue("@FirstName", model.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", model.LastName);

                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Dashboard");
        }

        return View(model);
    }

    public IActionResult DeleteUser()
    {
        List<CreateUserViewModel> users = new List<CreateUserViewModel>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            string query = "SELECT Username, Role, FirstName, LastName FROM Users";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new CreateUserViewModel
                        {
                            Username = reader["Username"].ToString(),
                            Role = reader["Role"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString()
                        });
                    }
                }
            }
        }

        return View(users);
    }

    [HttpPost]
    public IActionResult DeleteUser(string username)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            string query = "DELETE FROM Users WHERE Username = @Username";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.ExecuteNonQuery();
            }
        }

        return RedirectToAction("AdminDashboard");
    }
}
