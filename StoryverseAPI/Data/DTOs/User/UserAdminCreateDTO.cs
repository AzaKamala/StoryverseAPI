using System.ComponentModel.DataAnnotations;

namespace StoryverseAPI.Data.DTOs.User
{
    public class UserAdminCreateDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
