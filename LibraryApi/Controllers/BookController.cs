using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;
using LibraryApi.DTO;
using Swashbuckle.AspNetCore.Annotations;
using AutoMapper;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly LibraryContext _context;

        private readonly IMapper _mapper;

        public BookController(LibraryContext context, IMapper mapper)
        {
            _context = context;

            _mapper = mapper;
        }
        
        // GET: api/Book
        [HttpGet]
        [SwaggerOperation("Gets all existing books")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Returns list of all existing books", Type =typeof(BookWithUserDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "This happens only when the book table does not exist in the current database")]
        public async Task<ActionResult<IEnumerable<BookWithUserDTO>>> GetBook()
        {
          if (_context.Book == null)
          {
              return NotFound();
          }
            List<Book> books = await _context.Book
                        .Include(books=>books.User)
                        .ToListAsync();
            List<BookWithUserDTO> result = _mapper
                .Map<List<Book>, List<BookWithUserDTO>>(books);
            return result;
        }

        // GET: api/Book/Borrowed
        [HttpGet("Borrowed")]
        [SwaggerOperation("Gets all borrowed books")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Returns list of all borrowed books", Type =typeof(BookWithUserDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "This happens only when the book table does not exist in the current database")]
        public async Task<ActionResult<IEnumerable<BookWithUserDTO>>> GetBorrowedBook()
        {
            if (_context.Book == null)
            {
              return NotFound();
            }
            List<Book> books = await _context.Book
                        .Include(books=>books.User)
                        .Where(book=>book.UserId!=null)
                        .ToListAsync();

            List<BookWithUserDTO> result = _mapper
                .Map<List<Book>, List<BookWithUserDTO>>(books);
            return result;
        }

        /// <summary>
        /// Get book by id
        /// </summary>
        /// <returns> The book with the requested id </returns>
        /// <response code="200">Returns requested book</response>
        /// <response code="404">Either the table book doesn't exist, or requested book id doesn't exists</response>
        // GET: api/Book/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookWithUserDTO>> GetBook(long id)
        {
          if (_context.Book == null)
          {
              return NotFound();
          }
            var book = await _context.Book
                            .Include(b=>b.User)
                            .FirstOrDefaultAsync(b=>b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            BookWithUserDTO result = _mapper
                .Map<Book, BookWithUserDTO>(book);

            return result;
        }

        // GET: api/Book/name/foo
        [HttpGet("name/{name}")]
        public async Task<ActionResult<IEnumerable<BookWithUserDTO>>> GetBookByName(string name)
        {
            if (_context.Book == null)
            {
                return NotFound();
            }

            List<Book> books = await _context.Book
                        .Include(b=>b.User)
                        .Where(
                            b=>
                                b.SearchVector.Matches(name) ||
                                b.SearchVector
                                    .Matches(
                                        EF.Functions.PhraseToTsQuery("english",name)
                                    ) ||
                                b.SearchVector
                                    .Matches(
                                        EF.Functions.WebSearchToTsQuery("english",name)
                                    )
                        )
                        .ToListAsync();

            List<BookWithUserDTO> result = _mapper.Map<List<Book>, List<BookWithUserDTO>>(books);

            return result;
        }

        /// <summary>
        /// modify book entry
        /// </summary>
        /// <returns> Nothing </returns>
        /// <response code="204">Requested book is modified</response>
        /// <request code ="400">Requested book had already been disposed of</request>
        /// <response code="404">The requested book id doesn't exists</response>
        // PATCH: api/Book/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> PutBook(long id, ModifyBookDTO modifyBookDTO)
        {
            if (_context.Book == null)
            {
              return NotFound();
            }
            var book = await _context.Book.FirstOrDefaultAsync(b=>b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            if (book.DisposedAt != null)
            {
                return BadRequest();
            }
            book.Title = modifyBookDTO.Title ?? book.Title;
            book.Edition = modifyBookDTO.Edition ?? book.Edition;

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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

        /// <summary>
        /// Adds a new book entry
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     POST api/Book
        ///     {
        ///         "title": "foo",
        ///         "edition": "bar"
        ///     }
        /// </remarks>
        /// <returns> The newly added book entry </returns>
        /// <response code="201">Returns the newly created item</response>
        // POST: api/Book
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(201)]
        [Produces("application/json")]
        public async Task<ActionResult<BookDTO>> PostBook(CreateBookDTO createBookDTO)
        {
            if (_context.Book == null)
            {
                return Problem("Entity set 'LibraryContext.Book'  is null.");
            }
            
            Book book = new Book
            {
                Title= createBookDTO.Title,
                Edition= createBookDTO.Edition,
            };

            _context.Book.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                "GetBook", 
                new { id = book.Id },
                new BookDTO
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Edition = book.Edition,
                        AddedAt = book.AddedAt,
                    }
            );
        }

        /// <summary>
        /// Soft delete a book entry
        /// </summary>
        /// <returns> Nothing </returns>
        /// <response code="204">Requested book is soft-deleted</response>
        /// <response code="400">Trying to delete a book borrowed by an user</response>
        /// <response code="404">The requested book id doesn't exists</response>
        // DELETE: api/Book/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteBook(long id)
        {
            if (_context.Book == null)
            {
                return NotFound();
            }
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            
            if (book.UserId != null){
                return BadRequest();
            }

            book.DisposedAt = DateTime.Now;
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(long id)
        {
            return (_context.Book?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
