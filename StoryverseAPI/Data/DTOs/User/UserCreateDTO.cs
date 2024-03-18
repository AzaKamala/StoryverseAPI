using StoryverseAPI.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace StoryverseAPI.Data.DTOs.User
{
    public class UserCreateDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
