using AutoMapper;
using LibraryApi.Domain;
using LibraryApi.Models.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using LibraryApi.Filters;

namespace LibraryApi.Controllers
{
    public class BooksController : ControllerBase
    {
        private readonly LibraryDataContext _context;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _config;
        private readonly ILogger<BooksController> _logger;

        private readonly ILookupBooks _bookLookup;
        private readonly IBookCommands _bookCommands;


        public BooksController(LibraryDataContext context, IMapper mapper, MapperConfiguration config, ILogger<BooksController> logger, ILookupBooks bookLookup, IBookCommands bookCommands)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
            _logger = logger;
            _bookLookup = bookLookup;
            _bookCommands = bookCommands;
        }



        [HttpPut("/books/{id:int}/genre")]
        public async Task<ActionResult> UpdateGenre(int id, [FromBody] string genre)
        {
            var book = await _context.AvailableBooks.SingleOrDefaultAsync(b => b.Id == id);

            if (book != null)
            {
                book.Genre = genre; // One "Gotcha" - we aren't validating here.
                await _context.SaveChangesAsync();
                return Accepted(); // 202 means "I did it", could also use "No Content"
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("/books/{id:int}")]
        public async Task<ActionResult> RemoveBookFromInventory(int id)
        {
            await _bookCommands.RemoveBookAsync(id);

            return NoContent(); // this is a 204. A success. but there is no entity.
        }

        /// <summary>
        /// Use this to add a book to our inventory
        /// </summary>
        /// <param name="request">Which book you want to add</param>
        /// <returns>A new book!</returns>

        [HttpPost("/books")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 15)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ValidateModel(SerializeModelState = true)]
        public async Task<ActionResult<GetBookDetailsResponse>> AddABook([FromBody] PostBookRequest request)
        {
            // 1 Validate it. If Not - return a 400 Bad Request, optionally with some info
            // programmatic, imperative validation
            //***** THIS CHUNK OF CODE CAN BE DELETED BECAUSE VALIDATEMODEL HAS BEEN ADDED*********
            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/
            // 2. Save it in the database
            //    - Turn a PostBookRequest -> Book
            var bookToSave = _mapper.Map<Book>(request);
            //    - Add it to the Context
            _context.Books.Add(bookToSave);
            //    - Tell the context to save the changes. 
            await _context.SaveChangesAsync();
            //    - Turn that saved book into a GetBookDetailsResponse
            var response = _mapper.Map<GetBookDetailsResponse>(bookToSave);
            // 3. Return:
            //    - 201 Created Status Code
            //    - A location header with the Url of the newly created book.
            //    - A copy of the newly created book (what they would get if they followed the location)

            return CreatedAtRoute("books#getbookbyid", new { id = response.Id }, response);
        }

        [HttpGet("/books")]
        public async Task<ActionResult<GetBooksSummaryResponse>> GetAllBooks([FromQuery] string genre = null)
        {

            GetBooksSummaryResponse response = await _bookLookup.GetBooksByGenreAsync(genre);
            return Ok(response);
        }


        [HttpGet("/books/{id:int}", Name = "books#getbookbyid")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<GetBookDetailsResponse>> GetBookById(int id)
        {
            GetBookDetailsResponse book = await _bookLookup.GetBookByIdAsync(id);

            return this.Maybe(book);
        }
    }
}