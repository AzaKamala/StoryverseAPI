using Microsoft.AspNetCore.Mvc;
using StoryverseAPI.Data;
using StoryverseAPI.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace StoryverseAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private static readonly List<User> Users = new List<User>
        {
            new User { Id = 1, Username = "User1", Books = new List<Book>() },
            new User { Id = 2, Username = "User2", Books = new List<Book>() },
        };

        private DB _db;

        public UserController(DB db)
        {
            _db = db;
        }

        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return _db.Users.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public ActionResult<User> PostUser(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            var existingUser = _db.Users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Username = user.Username;
            existingUser.Books = user.Books;
            _db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            Users.Remove(user);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
