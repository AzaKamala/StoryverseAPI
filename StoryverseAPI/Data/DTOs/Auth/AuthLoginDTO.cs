using System.ComponentModel.DataAnnotations;

namespace StoryverseAPI.Data.DTOs.Auth
{
    public class AuthLoginDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
