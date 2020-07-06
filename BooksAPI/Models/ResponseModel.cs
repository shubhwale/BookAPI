using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksAPI.Models
{
    public class ResponseModel : Users
    {
        public ResponseModel(Users user)
        {
            this.UserId = user.UserId;
            this.Email = user.Email;
            this.Name = user.Name;
            this.Contact = user.Contact;
            this.Gender = user.Gender;
            this.AddressLine = user.AddressLine;
            this.CityId = user.CityId;
            this.StateId = user.StateId;
        }

        public string Token { get; set; }
    }
}
