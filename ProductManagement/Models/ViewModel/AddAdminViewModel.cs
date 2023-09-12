using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProductManagement.Models.ViewModel
{
    public class AddAdminViewModel
    {
        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }

        public string Role { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
