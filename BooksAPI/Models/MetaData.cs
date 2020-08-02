using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BooksAPI.Models
{
    public class MetaData
    {
        [Required(ErrorMessage ="Name required", AllowEmptyStrings = false)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email required", AllowEmptyStrings =false)]
        [EmailAddress(ErrorMessage ="Wrong format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password required", AllowEmptyStrings = false)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Contact required", AllowEmptyStrings = false)]
        public long Contact { get; set; }
        [Required(ErrorMessage = "Gender required", AllowEmptyStrings = false)]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Address Line required", AllowEmptyStrings = false)]
        public string AddressLine { get; set; }
        [Required(ErrorMessage = "City required", AllowEmptyStrings = false)]
        public int CityId { get; set; }
        [Required(ErrorMessage = "State required", AllowEmptyStrings = false)]
        public int StateId { get; set; }
    }
}
