namespace Contracts
{
    public class UpdatePasswordRequest
    {
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
