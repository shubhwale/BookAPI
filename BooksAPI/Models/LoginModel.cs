using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BooksAPI.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Please enter Email")]
        [EmailAddress(ErrorMessage ="Please enter valid Email address")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
