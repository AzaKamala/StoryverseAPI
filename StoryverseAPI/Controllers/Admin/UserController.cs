using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoryverseAPI.Data;
using StoryverseAPI.Data.DTOs.User;
using StoryverseAPI.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace StoryverseAPI.Controllers.Admin
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {

        private DB _db;

        public UserController(DB db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDTO>> GetUsers()
        {
            List<User> users = _db.Users.ToList();
            List<UserDTO> userDTOs = users.Select(user => new UserDTO(user)).ToList();

            return userDTOs;
        }

        [HttpGet("{id}")]
        public ActionResult<UserDTO> GetUser(int id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return new UserDTO(user);
        }

        [HttpPost]
        public ActionResult<UserDTO> PostUser(UserAdminCreateDTO user)
        {
            User newUser = new User
            {
                Username = user.Username,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role.ToLower() == "admin" ? Role.Admin : Role.User
            };

            _db.Users.Add(newUser);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, user);
        }

        [HttpPut("{id}")]
        public ActionResult PutUser(int id, UserDTO user)
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

            existingUser.Books = user.Books;
            _db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            _db.Users.Remove(user);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
