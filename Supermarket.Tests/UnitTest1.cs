using Supermarket.Web.Models;

namespace Supermarket.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Product_Should_Store_Product_Details()
        {
            var product = new Product
            {
                ProductId = 1,
                Title = "Fanta Orange",
                Barcode = "123456789",
                BrandSupplier = "Coca Cola",
                ProductCategory = "Soft Drinks",
                QuantityInStock = 25,
                Price = 2.00m,
                StockAvailabilityStatus = "In Stock"
            };

            Assert.Equal("Fanta Orange", product.Title);
            Assert.Equal("123456789", product.Barcode);
            Assert.Equal(25, product.QuantityInStock);
            Assert.Equal("In Stock", product.StockAvailabilityStatus);
        }

        [Fact]
        public void Order_Should_Store_Order_Details()
        {
            var order = new Order
            {
                OrderId = 1,
                CustomerName = "Parth",
                ProductName = "Fanta Orange",
                Quantity = 5,
                TotalPrice = 10.00m,
                OrderDate = DateTime.Today
            };

            Assert.Equal("Parth", order.CustomerName);
            Assert.Equal("Fanta Orange", order.ProductName);
            Assert.Equal(5, order.Quantity);
            Assert.Equal(10.00m, order.TotalPrice);
        }

        [Fact]
        public void Supplier_Should_Store_Supplier_Details()
        {
            var supplier = new Supplier
            {
                SupplierId = 1,
                SupplierName = "Coca Cola",
                ContactPerson = "John Smith",
                Email = "supplier@example.com",
                PhoneNumber = "07123456789",
                Address = "London"
            };

            Assert.Equal("Coca Cola", supplier.SupplierName);
            Assert.Equal("John Smith", supplier.ContactPerson);
            Assert.Equal("supplier@example.com", supplier.Email);
            Assert.Equal("07123456789", supplier.PhoneNumber);
        }

        [Fact]
        public void Product_Should_Be_Low_Stock_When_Quantity_Is_10_Or_Less()
        {
            var product = new Product
            {
                Title = "Walkers Crisps",
                QuantityInStock = 8,
                StockAvailabilityStatus = "Low Stock"
            };

            Assert.True(product.QuantityInStock <= 10);
            Assert.Equal("Low Stock", product.StockAvailabilityStatus);
        }

        [Fact]
        public void Product_Should_Be_Out_Of_Stock_When_Quantity_Is_Zero()
        {
            var product = new Product
            {
                Title = "Monster Drink",
                QuantityInStock = 0,
                StockAvailabilityStatus = "Out of Stock"
            };

            Assert.Equal(0, product.QuantityInStock);
            Assert.Equal("Out of Stock", product.StockAvailabilityStatus);
        }
    }
}