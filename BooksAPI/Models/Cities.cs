using System;
using System.Collections.Generic;

namespace BooksAPI.Models
{
    public partial class Cities
    {
        public Cities()
        {
            Customers = new HashSet<Customers>();
        }

        public int CityId { get; set; }
        public string CityName { get; set; }

        public ICollection<Customers> Customers { get; set; }
    }
}
