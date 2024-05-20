using System.ComponentModel.DataAnnotations;

namespace StockMax.Domain.Models.Entity
{
    public class Product : Model<Guid>
    {
        [MaxLength(255), Required]
        public string Name { get; set; } = "";

        [Required]
        public int Quantity { get; set; } = 0;

        [Required]
        public string ImagePath { get; set; } = "";

        [Required]
        public string Description { get; set; } = "";

        [Required]
        public string Colors { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Status { get; set; }

        public string Vendor { get; set; }
        public string Label { get; set; }
        public string Category { get; set; }

        [Required]
        public decimal Value { get; set; } = 0;
    }
}