using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                return CreatedAtAction("GetUsers", new { id = user.UserId });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ValidationProblem();
            }
        }

        // POST: api/Users/login
        [HttpPost("login")]
        public async Task<ActionResult<UserCreateResponse>> LoginUser([FromBody] LoginRequest loginRequest)
        {
            try
            {
                UserCreateResponse userCreateResponse = null;

                Users user = await _context.Users.Where(u => u.Email == loginRequest.Email && u.Password == loginRequest.Password).FirstOrDefaultAsync();

                if (user != null)
                {
                    RefreshToken refreshToken = GenerateRefreshToken();
                    user.RefreshToken.Add(refreshToken);
                    await _context.SaveChangesAsync();

                    userCreateResponse = new UserCreateResponse();
                    userCreateResponse.RefreshToken = refreshToken.Token;
                    userCreateResponse.AccessToken = GenerateAccessToken(userCreateResponse.UserId);
                    userCreateResponse.UserId = user.UserId;
                    userCreateResponse.Name = user.Name;
                }

                if (userCreateResponse == null)
                {
                    return NotFound();
                }

                return Ok(userCreateResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        // POST: api/Users/refreshtoken
        [HttpPost("refreshtoken")]
        public async Task<ActionResult<UserCreateResponse>> RefreshToken([FromBody] RefreshRequest refreshRequest)
        {
            try
            {
                Users user = GetUserFromToken(refreshRequest.Token);

                if (user != null && ValidateRefreshToken(user, refreshRequest.Token))
                {
                    UserCreateResponse responseModel = new UserCreateResponse();
                    responseModel.AccessToken = GenerateAccessToken(responseModel.UserId);
                    return Ok(responseModel);
                }
                return NotFound();
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

                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Ok(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        // POST: api/Users/getusredetails
        [HttpGet("getuserdetails/{id}")]
        public async Task<ActionResult<UserDetails>> GetUserDetails([FromRoute] int id)
        {
            try
            {
                UserDetails obj = new UserDetails();
                var user = await _context.Users.FindAsync(id);
                if(user!=null)
                {
                    obj.UserId = user.UserId;
                    obj.Name = user.Name;
                    obj.Email = user.Email;
                    return Ok(obj);
                }
                return NotFound();
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