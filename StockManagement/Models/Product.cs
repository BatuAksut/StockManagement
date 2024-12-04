using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockManagement.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        public int UnitsInStock { get; set; }

        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
    }
}
