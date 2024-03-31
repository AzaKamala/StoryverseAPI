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
            this.Books = user.Books;
        }

        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<Models.Book> Books { get; set; }
    }
}
