using System;
using System.Collections.Generic;

namespace BooksAPI.Models
{
    public partial class States
    {
        public States()
        {
            Users = new HashSet<Users>();
        }

        public int StateId { get; set; }
        public string StateName { get; set; }

        public ICollection<Users> Users { get; set; }
    }
}
