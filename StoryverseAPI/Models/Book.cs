namespace StoryverseAPI.Models
{
    public class Book
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public User Author { get; set; }
        public List<Chapter> Chapters { get; set; }
    }
}
