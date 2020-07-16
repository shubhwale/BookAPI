using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksAPI.Models;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BooksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly BookAppContext _context;
        private readonly JWTSettings _jwtSettings;

        public UsersController(BookAppContext context, IOptions<JWTSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }

        private string GenerateAccessToken(int id)
        {
            //sign token here
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,Convert.ToString(id))
                }),
                Expires = DateTime.UtcNow.AddSeconds(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        //generate refresh token here
        private RefreshToken GenerateRefreshToken()
        {
            RefreshToken refreshToken = new RefreshToken();

            var randomNumber = new Byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken.Token = Convert.ToBase64String(randomNumber);
            }
            refreshToken.ExpiryDate = DateTime.UtcNow.AddMonths(6);

            return refreshToken;
        }

        //Testing purpose
        // GET: api/Users
        [HttpGet]
        public IEnumerable<Users> GetUsers()
        {
            try
            {
                return _context.Users.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        //GET: api/users/listcities
        [HttpGet("listcities")]
        public IEnumerable<Cities> GetCities()
        {
            try
            {
                return _context.Cities.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        //GET: api/users/liststates
        [HttpGet("liststates")]
        public IEnumerable<States> GetStates()
        {
            try
            {
                return _context.States.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        // POST: api/Users/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] Users user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _context.Users.Add(user);
                await _context.SaveChangesAsync();



                return CreatedAtAction("GetUsers", new { id = user.UserId }, user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ValidationProblem();
            }
        }

        // POST: api/Users/login
        [HttpPost("login")]
        public async Task<ActionResult<ResponseModel>> LoginUser([FromBody] Users user)
        {
            try
            {
                user = await _context.Users.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefaultAsync();
                ResponseModel responseModel = null;

                if (user != null)
                {
                    RefreshToken refreshToken = GenerateRefreshToken();
                    user.RefreshToken.Add(refreshToken);
                    await _context.SaveChangesAsync();

                    responseModel = new ResponseModel(user);
                    responseModel.RefreshToken = refreshToken.Token;
                }

                if (responseModel == null)
                {
                    return NotFound();
                }
                responseModel.AccessToken = GenerateAccessToken(responseModel.UserId);

                return responseModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        // POST: api/Users/refreshtoken
        [HttpPost("refreshtoken")]
        public async Task<ActionResult<ResponseModel>> RefreshToken([FromBody] RefreshRequest refreshRequest)
        {
            try
            {
                Users user = GetUserFromToken(refreshRequest.Token);

                if (user != null && ValidateRefreshToken(user, refreshRequest.Token))
                {
                    ResponseModel responseModel = new ResponseModel(user);
                    responseModel.AccessToken = GenerateAccessToken(responseModel.UserId);
                    return responseModel;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private bool ValidateRefreshToken(Users user, string refreshToken)
        {
            RefreshToken refreshTokenUser = _context.RefreshToken.Where(rt => rt.Token == refreshToken)
                                            .OrderByDescending(rt => rt.ExpiryDate)
                                            .FirstOrDefault();

            if (refreshTokenUser != null && refreshTokenUser.UserId == user.UserId && refreshTokenUser.ExpiryDate > DateTime.UtcNow)
            {
                return true;
            }

            return false;
        }

        private Users GetUserFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            SecurityToken securityToken;

            var principle = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken != null && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                var userId = principle.FindFirst(ClaimTypes.Name)?.Value;
                return _context.Users.Where(u => u.UserId == Convert.ToInt32(User)).FirstOrDefault();
            }

            return null;
        }


        // PUT: api/Users/5
        [HttpPut("{id}")]
        //public async Task<IActionResult> EditUser([FromRoute] int id, [FromBody] Users users)
        public async Task<IActionResult> UpdateUser([FromBody] Users users)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //if (id != users.UserId)
                //{
                //    return BadRequest();
                //}

                _context.Entry(users).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.UserId))
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
                return null;
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var users = await _context.Users.FindAsync(id);
                if (users == null)
                {
                    return NotFound();
                }

                _context.Users.Remove(users);
                await _context.SaveChangesAsync();

                return Ok(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}