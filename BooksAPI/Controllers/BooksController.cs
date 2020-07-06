using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksAPI.Models;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace BooksAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookAppContext context;

        public BooksController(BookAppContext context)
        {
            this.context = context;
        }

        // GET: api/books OR
        // GET: api/books?categoryid=1
        [HttpGet]
        public IActionResult ListBooksByCategory([FromQuery(Name = "categoryid")] int categoryId = 0)
        {
            var categoryIdParameter = new SqlParameter("@CategoryId", categoryId);
            try
            {
                if(categoryId == 0)
                {
                    return Ok(context.Books.ToList());
                }
                return Ok(context.Books.FromSql("SP_GetBooksByCategoryId @CategoryId", categoryIdParameter).ToList());
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook([FromRoute] int id)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        //public async Task<IActionResult> EditBook([FromRoute] int id, [FromBody] Books books)
        public async Task<IActionResult> EditBook([FromBody] Books books)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //if (id != books.BookId)
                //{
                //    return BadRequest();
                //}

                context.Entry(books).State = EntityState.Modified;

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BooksExists(books.BookId))
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        // POST: api/Books
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] Books books)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook([FromRoute] int id)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        private bool BooksExists(int id)
        {
            return context.Books.Any(e => e.BookId == id);
        }
    }
}