using System.ComponentModel.DataAnnotations;
namespace InthinkSolution.Models
{
    public class CreateUserViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Email { get; set; }

        [Required]
        public string Role { get; set; } // Dropdown for roles

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}