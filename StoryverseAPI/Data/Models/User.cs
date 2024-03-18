namespace StoryverseAPI.Data.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Book> Books { get; set; }
    }
}
