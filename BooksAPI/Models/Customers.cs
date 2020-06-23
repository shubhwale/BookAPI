using System;
using System.Collections.Generic;

namespace BooksAPI.Models
{
    public partial class Customers
    {
        public int CustId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public long Contact { get; set; }
        public string Gender { get; set; }
        public string AddressLine { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }

        public Cities City { get; set; }
        public States State { get; set; }
    }
}
