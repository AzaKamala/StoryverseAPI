namespace StoryverseAPI.Data.Models
{
    public class Chapter
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Book Book { get; set; }
    }
}
