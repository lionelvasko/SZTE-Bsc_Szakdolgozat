using System.ComponentModel.DataAnnotations;

namespace Szakdoga.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        internal string ConfirmPassword { get; set; }
    }
}
