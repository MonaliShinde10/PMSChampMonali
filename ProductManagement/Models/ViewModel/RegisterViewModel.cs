using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProductManagement.Models.ViewModel
{
    public class RegisterViewModel 
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
     

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; } = string.Empty;

    }
}
