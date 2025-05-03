namespace AuthAPI.DTOs
{
    /// <summary>  
    /// Represents a Data Transfer Object for a user.  
    /// </summary>  
    public class UserDTO
    {
        /// <summary>  
        /// Gets or sets the email address of the user.  
        /// </summary>  
        public required string Email { get; set; }

        /// <summary>  
        /// Gets or sets the first name of the user.  
        /// </summary>  
        public required string FirstName { get; set; }

        /// <summary>  
        /// Gets or sets the last name of the user.  
        /// </summary>  
        public required string LastName { get; set; }
    }
}
