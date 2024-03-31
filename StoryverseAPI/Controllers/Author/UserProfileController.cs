using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoryverseAPI.Data;
using StoryverseAPI.Data.DTOs.Book;
using StoryverseAPI.Data.DTOs.Chapter;
using StoryverseAPI.Data.DTOs.User;
using StoryverseAPI.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace StoryverseAPI.Controllers.Admin
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "User, Admin")]
    public class UserProfileController : ControllerBase
    {
        private DB _db;

        public UserProfileController(DB db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<UserDTO> Get()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim.Value);

            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            return new UserDTO(user);
        }

        // User Books Routes
        //

        [HttpGet("/books")]
        public ActionResult<IEnumerable<BookDTO>> GetUserBooks()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim.Value);

            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            return user.Books.Select(book => new BookDTO(book)).ToList();
        }

        [HttpGet("/books/{bookId}")]
        public ActionResult<BookDTO> GetUserBook(int bookId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim.Value);

            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            var book = user.Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                return NotFound();
            }

            return new BookDTO(book);
        }

        [HttpPost("/books")]
        public ActionResult<BookDTO> AddBookToUser(BookCreateDTO book)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim.Value);

            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            var newBook = new Book
            {
                Title = book.Title,
                Author = user
            };
            _db.Books.Add(newBook);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetUserBook), new { bookId = newBook.Id });
        }

        [HttpPut("/books/{bookId}")]
        public ActionResult EditUserBook(int bookId, BookDTO book)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim.Value);

            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            var existingBook = user.Books.FirstOrDefault(b => b.Id == bookId);
            if (existingBook == null)
            {
                return NotFound();
            }

            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.Chapters = book.Chapters;

            _db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("/books/{bookId}")]
        public ActionResult DeleteUserBook(int bookId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim.Value);

            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            var book = user.Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                return NotFound();
            }

            _db.Books.Remove(book);
            _db.SaveChanges();

            return NoContent();
        }

        // Book Chapters Routes
        //

        [HttpGet("/books/{bookId}/chapters")]
        public ActionResult<IEnumerable<ChapterDTO>> GetBookChapters(int bookId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim.Value);

            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            var book = user.Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                return NotFound();
            }

            return book.Chapters.Select(chapter => new ChapterDTO(chapter)).ToList();
        }

        [HttpGet("/books/{bookId}/chapters/{chapterId}")]
        public ActionResult<ChapterDTO> GetBookChapter(int bookId, int chapterId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim.Value);

            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            var book = user.Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                return NotFound();
            }

            var chapter = book.Chapters.FirstOrDefault(c => c.Id == chapterId);
            if (chapter == null)
            {
                return NotFound();
            }

            return new ChapterDTO(chapter);
        }

        [HttpPost("/books/{bookId}/chapters")]
        public ActionResult<ChapterDTO> AddChapterToBook(int bookId, ChapterCreateDTO chapter)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim.Value);

            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            var book = user.Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                return NotFound();
            }

            var newChapter = new Chapter
            {
                Title = chapter.Title,
                Content = chapter.Content,
                Book = book,
            };
            _db.Chapters.Add(newChapter);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetBookChapter), new { bookId = book.Id, chapterId = newChapter.Id });
        }

        [HttpPut("/books/{bookId}/chapters/{chapterId}")]
        public ActionResult EditBookChapter(int bookId, int chapterId, ChapterDTO chapter)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim.Value);

            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            var book = user.Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                return NotFound();
            }

            var existingChapter = book.Chapters.FirstOrDefault(c => c.Id == chapterId);
            if (existingChapter == null)
            {
                return NotFound();
            }

            existingChapter.Title = chapter.Title;
            existingChapter.Content = chapter.Content;

            _db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("/books/{bookId}/chapters/{chapterId}")]
        public ActionResult DeleteBookChapter(int bookId, int chapterId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim.Value);

            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            var book = user.Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                return NotFound();
            }

            var chapter = book.Chapters.FirstOrDefault(c => c.Id == chapterId);
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
