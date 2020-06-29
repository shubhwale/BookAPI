using System;
using System.Collections.Generic;

namespace BooksAPI.Models
{
    public partial class BooksCategories
    {
        public int Bid { get; set; }
        public int Cid { get; set; }

        public Books B { get; set; }
        public Categories C { get; set; }
    }
}
