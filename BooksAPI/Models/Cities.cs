using System;
using System.Collections.Generic;

namespace BooksAPI.Models
{
    public partial class Cities
    {
        public Cities()
        {
            Users = new HashSet<Users>();
        }

        public int CityId { get; set; }
        public string CityName { get; set; }

        public ICollection<Users> Users { get; set; }
    }
}
