using System;
using System.Collections.Generic;

namespace FPTBookStore.Models
{
    public partial class Book
    {
        public int BookId { get; set; }
        public string? BookName { get; set; }
        public string? BookDescription { get; set; }
        public string? BookAuthor { get; set; }
        public decimal? BookPrice { get; set; }
        public string? BookImage { get; set; }
        public int? GenreId { get; set; }
        public int? BookSells { get; set; }
        public int? BookStock { get; set; }
    }
}
