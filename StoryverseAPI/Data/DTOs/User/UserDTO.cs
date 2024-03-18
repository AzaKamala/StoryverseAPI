using StoryverseAPI.Data.Models;

namespace StoryverseAPI.Data.DTOs.User
{
    public class UserDTO
    {
        public UserDTO(Models.User user)
        {
            this.Id = user.Id;
            this.Username = user.Username;
            this.Email = user.Email;
            this.Password = user.Password;
            this.Books = user.Books;
        }

        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Book> Books { get; set; }
    }
}
