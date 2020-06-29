using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksAPI.Models;
using System.Data.SqlClient;

namespace BooksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookAppContext context;

        public BooksController(BookAppContext context)
        {
            this.context = context;
        }

        // GET: api/books
        [HttpGet]
        public IActionResult GetBooks([FromQuery(Name ="category")]string category = "All")
        {
            try
            {
                //context.BooksCategories.Include(bc => bc.Book).ToList()
                //context.Books.ToList()
                string c = category.ToLower();
                var categoryParam = new SqlParameter("@CategoryName", c);
                switch (c)
                {
                    case "all": return Ok(context.Books.ToList());
                    case "programming":
                        //var obj = context.Books.FromSql("SP_GetBooksByCategory @CategoryName", categoryParam).ToList();
                        return Ok(context.Books.FromSql("SP_GetBooksByCategory @CategoryName", categoryParam).ToList());
                    case "biographies": return Ok(context.Books.FromSql("SP_GetBooksByCategory @CategoryName", categoryParam).ToList());
                    case "business": return Ok(context.Books.FromSql("SP_GetBooksByCategory @CategoryName", categoryParam).ToList());
                    case "technology": return Ok(context.Books.FromSql("SP_GetBooksByCategory @CategoryName", categoryParam).ToList());
                    case "history": return Ok(context.Books.FromSql("SP_GetBooksByCategory @CategoryName", categoryParam).ToList());
                    case "self_help": return Ok(context.Books.FromSql("SP_GetBooksByCategory @CategoryName", categoryParam).ToList());
                    default: return Ok(context.Books.FromSql("SP_GetBooksByCategory @CategoryName", categoryParam).ToList());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooks([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = await context.Books.FindAsync(id);

            

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooks([FromRoute] int id, [FromBody] Books books)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != books.BookId)
            {
                return BadRequest();
            }

            context.Entry(books).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BooksExists(id))
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

        // POST: api/Books
        [HttpPost]
        public async Task<IActionResult> PostBooks([FromBody] Books books)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Books.Add(books);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BooksExists(books.BookId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBooks", new { id = books.BookId }, books);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooks([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var books = await context.Books.FindAsync(id);
            if (books == null)
            {
                return NotFound();
            }

            context.Books.Remove(books);
            await context.SaveChangesAsync();

            return Ok(books);
        }

        private bool BooksExists(int id)
        {
            return context.Books.Any(e => e.BookId == id);
        }
    }
}