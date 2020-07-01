using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksAPI.Models
{
    public class CustomerWithToken : Customers
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Token { get; internal set; }

        public CustomerWithToken( Customers customer)
        {
            this.CustId = customer.CustId;
            this.Name = customer.Name;
            this.Email = customer.Email;
            this.Password = customer.Password;
            this.Contact = customer.Contact;
            this.Gender = customer.Gender;
            this.AddressLine = customer.AddressLine;
            this.CityId = customer.CityId;
            this.StateId = customer.StateId;

            this.City = customer.City;
            this.State = customer.State;
        }
    }
}
