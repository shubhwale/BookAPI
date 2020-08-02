using System;
using System.Collections.Generic;

namespace BooksAPI.Models
{
    public partial class Categories
    {
        public Categories()
        {
            BooksCategories = new HashSet<BooksCategories>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<BooksCategories> BooksCategories { get; set; }
    }
}
