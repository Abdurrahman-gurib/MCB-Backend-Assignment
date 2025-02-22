using System.ComponentModel.DataAnnotations;

namespace MCBBackend.Models
{
    public class User
    {
        [Key]
        public string Username { get; set; }
        public string Password { get; set; } // In production, store a hashed password!
        public string Role { get; set; } // e.g., "Admin", "User"
    }
}
