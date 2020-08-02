using System;
using System.Collections.Generic;

namespace BooksAPI.Models
{
    public partial class BooksCategories
    {
        public int Bid { get; set; }
        public int Cid { get; set; }

        public virtual Books B { get; set; }
        public virtual Categories C { get; set; }
    }
}
