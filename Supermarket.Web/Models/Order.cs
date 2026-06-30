using System.ComponentModel.DataAnnotations;

namespace Supermarket.Web.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [Required]
        public string CustomerName { get; set; } = "";

        [Required]
        public string ProductName { get; set; } = "";

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }
    }
}