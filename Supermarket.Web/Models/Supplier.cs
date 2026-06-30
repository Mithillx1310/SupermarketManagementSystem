using System.ComponentModel.DataAnnotations;

namespace Supermarket.Web.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }

        [Required]
        public string SupplierName { get; set; } = "";

        [Required]
        public string ContactPerson { get; set; } = "";

        [Required]
        public string PhoneNumber { get; set; } = "";

        public string Email { get; set; } = "";

        public string Address { get; set; } = "";
    }
}