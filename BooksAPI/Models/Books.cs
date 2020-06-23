using System;
using System.Collections.Generic;

namespace BooksAPI.Models
{
    public partial class Books
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public int NoOfPages { get; set; }
        public int Rating { get; set; }
        public int Edition { get; set; }
        public int Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ImageUrl { get; set; }
    }
}
