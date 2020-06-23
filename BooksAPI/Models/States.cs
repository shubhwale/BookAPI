using System;
using System.Collections.Generic;

namespace BooksAPI.Models
{
    public partial class States
    {
        public States()
        {
            Customers = new HashSet<Customers>();
        }

        public int StateId { get; set; }
        public string StateName { get; set; }

        public ICollection<Customers> Customers { get; set; }
    }
}
