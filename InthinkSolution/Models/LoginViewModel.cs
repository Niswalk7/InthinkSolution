using System.ComponentModel.DataAnnotations;

namespace InthinkSolution.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }
    }

    public class User
    {
      
        public int Id { get; set; } // Primary Key
        public string? Username { get; set; } 

        public string? Email { get; set; }

        
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        
        public string? Role { get; set; } // Dropdown for roles

        
        public string? FirstName { get; set; }

        
        public string? LastName { get; set; }
    }

    public class ResetPasswordViewModel
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

}
