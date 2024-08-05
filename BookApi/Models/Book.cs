using System.ComponentModel.DataAnnotations;

namespace Books.Models
{
    public class Book
    {
        [Key]
        public required long ISBN { get; set; }
        public required string Title { get; set; }

        public required string Publisher { get; set; }

        public required string Authors { get; set; }
    }
}
