using System.ComponentModel.DataAnnotations;

namespace FPTBookStore.Models
{
    public class AddedBook
    {
        public int BookId { get; set; }
        public string? BookName { get; set; }
        public string? BookDescription { get; set; }
        public string? BookAuthor { get; set; }
        public decimal? BookPrice { get; set; }
        public int? GenreId { get; set; }
        public int? BookStock { get; set; }
        public IFormFile? BookImage { get; set; }
        public int? BookSells { get; set; }
    }
}
