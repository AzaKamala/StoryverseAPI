using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoryverseAPI.Data;
using StoryverseAPI.Data.DTOs.Book;
using StoryverseAPI.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace StoryverseAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BookController : ControllerBase
    {
        private DB _db;

        public BookController(DB db)
        {
            _db = db;
        }

        [HttpGet]
        public IEnumerable<BookDTO> GetBooks()
        {
            List<Book> books = _db.Books.ToList();
            List<BookDTO> bookDTOs = books.Select(book => new BookDTO(book)).ToList();

            return bookDTOs;
        }

        [HttpGet("{id}")]
        public ActionResult<BookDTO> GetBook(long id)
        {
            var book = _db.Books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return new BookDTO(book);
        }

        [HttpPost]
        public ActionResult<BookDTO> PostBook(BookCreateDTO book)
        {
            User author = _db.Users.FirstOrDefault(u => u.Id == book.AuthorId);
            if (author == null)
            {
                return NotFound();
            }

            Book newBook = new Book
            {
                Title = book.Title,
                Author = author,
            };

            _db.Books.Add(newBook);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, book);
        }

        [HttpPut("{id}")]
        public IActionResult PutBook(long id, BookDTO book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            var existingBook = _db.Books.FirstOrDefault(b => b.Id == id);
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

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(long id)
        {
            var book = _db.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            _db.Books.Remove(book);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
