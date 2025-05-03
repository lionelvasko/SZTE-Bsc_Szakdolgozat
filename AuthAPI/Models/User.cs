using Microsoft.AspNetCore.Identity;

namespace AuthAPI.Models
{
    /// <summary>
    /// Represents a user in the authentication system.
    /// Inherits from IdentityUser and includes additional properties such as FirstName, LastName, and Devices.
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of devices associated with the user.
        /// </summary>
        public ICollection<Device> Devices { get; set; } = new List<Device>();
    }
}
