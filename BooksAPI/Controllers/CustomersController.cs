using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BooksAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly BookAppContext _context;
        private readonly JWTSettings _jwtSettings;

        public CustomersController(BookAppContext context,IOptions<JWTSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }

        // GET: api/Customers
        [HttpGet]
        public IEnumerable<Customers> GetCustomers()
        {
            return _context.Customers.ToList();
        }

        //GET: api/customers/login
        [HttpGet("login")]
        public async Task<ActionResult<CustomerWithToken>> Login([FromBody] Customers customer)
        {
            customer = await _context.Customers.Where(c => c.Email == customer.Email && c.Password == customer.Password).FirstOrDefaultAsync();
            CustomerWithToken customerWithToken = new CustomerWithToken(customer);

            if(customerWithToken == null)
            {
                return NotFound();
            }

            //sign token here
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,customer.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            customerWithToken.Token = tokenHandler.WriteToken(token);

            return customerWithToken;
        }

        // GET: api/Customers
        [HttpGet("getcust")]
        public async Task<ActionResult<Customers>> GetCustAsync()
        {
            string emailAddress = HttpContext.User.Identity.Name;
            var cust = await _context.Customers.Where(c => c.Email == emailAddress).FirstOrDefaultAsync();
            cust.Password = null;
            return Ok(cust);
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customers = await _context.Customers.FindAsync(id);

            if (customers == null)
            {
                return NotFound();
            }

            return Ok(customers);
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomers([FromRoute] int id, [FromBody] Customers customers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customers.CustId)
            {
                return BadRequest();
            }

            _context.Entry(customers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomersExists(id))
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

        // POST: api/Customers
        [HttpPost]
        public async Task<IActionResult> PostCustomers([FromBody] Customers customers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Customers.Add(customers);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomers", new { id = customers.CustId }, customers);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customers = await _context.Customers.FindAsync(id);
            if (customers == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customers);
            await _context.SaveChangesAsync();

            return Ok(customers);
        }

        private bool CustomersExists(int id)
        {
            return _context.Customers.Any(e => e.CustId == id);
        }
    }
}