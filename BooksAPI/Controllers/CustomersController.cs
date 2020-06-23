using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksAPI.Models;

namespace BooksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        // GET api/customers
        [HttpGet]
        public IActionResult GetCustomers()
        {
            try
            {
                using (var context = new BookAppContext())
                {
                    return Ok(context.Customers.ToList());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        // GET api/customers
        [HttpGet("{id}")]
        public IActionResult GetCustomers([FromRoute] int id)
        {
            try
            {
                using (var context = new BookAppContext())
                {
                    return Ok(context.Customers.FirstOrDefault(c => c.CustId == id));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        // POST: api/customers/signin
        [HttpPost("signin")]
        public IActionResult Signin([FromBody] Customers customer)
        {
            using (var context = new BookAppContext())
            {
                try
                {
                    var email = context.Customers.FirstOrDefault(c => c.Email == customer.Email);
                    var password = context.Customers.FirstOrDefault(c => c.Password == customer.Password);
                    if (email != null && password != null)
                    {
                        return Ok();
                    }
                    return NotFound();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return NotFound();
                }
            }
        }

        // POST: api/customers/signup
        [HttpPost("signup")]
        public IActionResult Signup([FromBody] Customers customer)
        {
            try
            {
                using (var context = new BookAppContext())
                {
                    context.Customers.Add(customer);
                    context.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
    }
}