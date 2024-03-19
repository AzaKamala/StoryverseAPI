using System.ComponentModel.DataAnnotations;

namespace StoryverseAPI.Data.DTOs.Auth
{
    public class AuthRegisterDTO
    {
        [Required]
        public string Username { get; set; }
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
