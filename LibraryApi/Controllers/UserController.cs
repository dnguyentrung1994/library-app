using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;
using LibraryApi.DTO;
using AutoMapper;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly LibraryContext _context;

        private readonly IMapper _mapper;


        public UserController(LibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>List of all users</returns>
        /// <response code="200">List of all users</response>
        /// <response code="404">In case the "user" table is not found in database</response>
        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserWithBookDTO>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            List<User> users = await _context.Users
                    .Include(u=>u.Books)
                    .ToListAsync();

            List<UserWithBookDTO> result = _mapper.Map<List<User>, List<UserWithBookDTO>>(users);

            return result;
        }

        /// <summary>
        /// Get all currently active users
        /// </summary>
        /// <returns>List of all users</returns>
        /// <response code="200">List of all active users</response>
        /// <response code="404">In case the "user" table is not found in database</response>
        // GET: api/User
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<UserWithBookDTO>>> GetActiveUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            List<User> users = await _context.Users
                    .Include(u=>u.Books)
                    .Where(u=>u.IsActive == true)
                    .ToListAsync();

            List<UserWithBookDTO> result = _mapper.Map<List<User>, List<UserWithBookDTO>>(users);

            return result;
        }
        /// <summary>
        /// Get specific user by ID
        /// </summary>
        /// <param name="id">the id of the user</param>
        /// <returns>User info</returns>
        /// <response code="200">Returns requested user</response>
        /// <response code="404">Either the table user doesn't exist, or requested user id doesn't exists</response>
        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserWithBookDTO>> GetUser(long id)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            var user = await _context.Users
                    .Include(u=>u.Books)
                    .Where(u=>u.Id == id)
                    .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }   else 
            {
                UserWithBookDTO result = _mapper.Map<User, UserWithBookDTO>(user);
                return result;
            }


        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
          if (_context.Users == null)
          {
              return Problem("Entity set 'LibraryContext.Users'  is null.");
          }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(long id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
