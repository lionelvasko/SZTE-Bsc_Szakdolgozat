using System.ComponentModel.DataAnnotations;

namespace Szakdoga.Requests
{
    public class PasswordChangeRequest
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
