
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BooksApi.Data;
using BooksApi.Models;
using BooksApi.Models.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BooksApi.Controllers
{
    public class BooksController : ControllerBase
    {
        private readonly LibraryDataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _config;

        public BooksController(LibraryDataContext dataContext, IMapper mapper, MapperConfiguration config)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _config = config;
        }

        [HttpGet("books")]
        public async Task<ActionResult<CollectionBase<GetBookSummaryItemResponse>>> GetAllBooks()
        {

            var response = new CollectionBase<GetBookSummaryItemResponse>();

            response.Data = await _dataContext
                .GetBooksInInventory()
                .ProjectTo<GetBookSummaryItemResponse>(_config)
                .ToListAsync();

            return Ok(response);

        }

        [HttpGet("books/{id:int}", Name ="books-getbookbyid")]
        public async Task<ActionResult<GetBookResponse>> GetBookById(int id)
        {
            var response = await _dataContext.GetBookById(id)
                .ProjectTo<GetBookResponse>(_config)
                .SingleOrDefaultAsync();

            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }

        }

        [HttpPost("books")]
        public async Task<ActionResult<GetBookResponse>> AddBook([FromBody] PostBookRequest request)
        {
            // When posting to a collection:
            // 1. Validate (if not valid, return a 400)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400
            }
            else
            {
                // 2. Add it to the database.
                var bookToAdd = _mapper.Map<Book>(request);
                _dataContext.Books.Add(bookToAdd);
                await _dataContext.SaveChangesAsync();
                // 3. Return a 201, with a Location header of the new resource
                // 4. And be nice if you can and just give it to them.
                var response = _mapper.Map<GetBookResponse>(bookToAdd);
                return CreatedAtRoute("books-getbookbyid", new { id = response.Id }, response);
            }

        }
        [HttpDelete("books/{id:int}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var book = await _dataContext.Books
                .Where(b => b.Id == id && !b.RemovedFromInventory)
                .SingleOrDefaultAsync();

            if(book != null)
            {
                book.RemovedFromInventory = true;
                await _dataContext.SaveChangesAsync();
            }

            return NoContent(); // Idempotent constraint.
        }

        [HttpPut("books/{id:int}/title")]
        public async Task<ActionResult> UpdateTitle(int id, [FromBody] string title)
        {
            var book = await _dataContext.GetBookById(id).SingleOrDefaultAsync();
            if(book == null)
            {
                return NotFound();
            } else
            {
                book.Title = title; // we aren't validating here.
                await _dataContext.SaveChangesAsync();
                return NoContent();
            }
        }

        [HttpPut("books/{id:int}/author")]
        public async Task<ActionResult> UpdateAuthor(int id, [FromBody] string author)
        {
            var book = await _dataContext.GetBookById(id).SingleOrDefaultAsync();
            if (book == null)
            {
                return NotFound();
            }
            else
            {
                book.Author = author; // we aren't validating here.
                await _dataContext.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
