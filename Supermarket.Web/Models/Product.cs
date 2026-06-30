using System;
using System.ComponentModel.DataAnnotations;

namespace Supermarket.Web.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        public string Title { get; set; } = "";

        [Required]
        public string Barcode { get; set; } = "";

        [Required]
        public string BrandSupplier { get; set; } = "";

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public string ProductCategory { get; set; } = "";

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int QuantityInStock { get; set; }

        [Required]
        public string StockAvailabilityStatus { get; set; } = "";
    }
}
