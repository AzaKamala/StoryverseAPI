using System.ComponentModel.DataAnnotations;

namespace StoryverseAPI.Data.DTOs.Book
{
    public class CreateBookDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public long AuthorId { get; set; }
    }
}
