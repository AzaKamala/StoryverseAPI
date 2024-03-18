using StoryverseAPI.Data.Models;

namespace StoryverseAPI.Data.DTOs.Book
{
    public class BookDTO
    {
        public BookDTO(Models.Book book)
        {
            Id = book.Id;
            Title = book.Title;
            Author = book.Author;
            Chapters = book.Chapters;
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public Models.User Author { get; set; }
        public List<Chapter> Chapters { get; set; }
    }
}
