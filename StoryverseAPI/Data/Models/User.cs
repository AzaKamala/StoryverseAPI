namespace StoryverseAPI.Data.Models
{
    public enum Role
    {
        Admin,
        User
    }

    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Book> Books { get; set; }
        public Role Role { get; set; } = Role.User;
    }
}
