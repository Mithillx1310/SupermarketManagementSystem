using System;
using System.ComponentModel.DataAnnotations;

namespace Supermarket.Web.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Barcode { get; set; } = string.Empty;

        [Required]
        public string BrandSupplier { get; set; } = string.Empty;

        public DateTime ExpiryDate { get; set; }

        [Required]
        public string ProductCategory { get; set; } = string.Empty;

        public string StockAvailabilityStatus { get; set; } = "In Stock";

        [Range(0.01, 9999)]
        public decimal Price { get; set; }

        [Range(0, 10000)]
        public int QuantityInStock { get; set; }
    }
}