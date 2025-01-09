using System.ComponentModel.DataAnnotations;

namespace InthinkSolution.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }
    }

    public class User
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }

}
