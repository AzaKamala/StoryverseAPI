namespace StoryverseAPI.Data.DTOs.Chapter
{
    public class ChapterDTO
    {
        public ChapterDTO(Models.Chapter chapter)
        {
            Id = chapter.Id;
            Title = chapter.Title;
            Content = chapter.Content;
            Book = chapter.Book;
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Models.Book Book { get; set; }
    }
}
