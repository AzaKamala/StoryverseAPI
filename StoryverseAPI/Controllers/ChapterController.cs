using Microsoft.AspNetCore.Mvc;
using StoryverseAPI.Data;
using StoryverseAPI.Data.DTOs.Chapter;
using StoryverseAPI.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace StoryverseAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChapterController : ControllerBase
    {
        private DB _db;

        public ChapterController(DB db)
        {
            _db = db;
        }

        [HttpGet]
        public IEnumerable<ChapterDTO> GetChapters()
        {
            List<Chapter> chapters = _db.Chapters.ToList();
            List<ChapterDTO> chapterDTOs = chapters.Select(chapter => new ChapterDTO(chapter)).ToList();

            return chapterDTOs;
        }

        [HttpGet("{id}")]
        public ActionResult<ChapterDTO> GetChapter(long id)
        {
            var chapter = _db.Chapters.FirstOrDefault(c => c.Id == id);

            if (chapter == null)
            {
                return NotFound();
            }

            return new ChapterDTO(chapter);
        }

        [HttpPost]
        public ActionResult<ChapterDTO> PostChapter(ChapterCreateDTO chapter)
        {
            Book book = _db.Books.FirstOrDefault(b => b.Id == chapter.BookId);
            if (book == null)
            {
                return NotFound();
            }

            Chapter newChapter = new Chapter
            {
                Title = chapter.Title,
                Content = chapter.Content,
                Book = book
            };

            _db.Chapters.Add(newChapter);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetChapter), new { id = newChapter.Id }, chapter);
        }

        [HttpPut("{id}")]
        public IActionResult PutChapter(long id, ChapterDTO chapter)
        {
            if (id != chapter.Id)
            {
                return BadRequest();
            }

            var existingChapter = _db.Chapters.FirstOrDefault(c => c.Id == id);
            if (existingChapter == null)
            {
                return NotFound();
            }

            existingChapter.Title = chapter.Title;
            existingChapter.Content = chapter.Content;
            existingChapter.Book = chapter.Book;
            _db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteChapter(long id)
        {
            var chapter = _db.Chapters.FirstOrDefault(c => c.Id == id);
            if (chapter == null)
            {
                return NotFound();
            }

            _db.Chapters.Remove(chapter);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
