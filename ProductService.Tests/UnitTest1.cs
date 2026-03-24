using ProductService.Core.Domain;
using Xunit;

namespace ProductService.Tests
{
    public class ProductTests
    {
        [Fact] // Bu bir test metodudur demek
        public void Product_Should_Be_Created_With_Correct_Data()
        {
            // 1. Hazırlık (Arrange)
            var product = new Product { Name = "Kulaklık", Price = 500, Stock = 10 };

            // 2. Kontrol Et (Assert)
            Assert.Equal("Kulaklık", product.Name);
            Assert.Equal(500, product.Price);
        }
    }
}