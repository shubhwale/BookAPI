using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BooksAPI.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email required", AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage = "Wrong format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password required", AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}
