using Product.Api.Entities;
using ILogger = Serilog.ILogger;

namespace Product.Api.Persistence
{
    public class ProductContextSeed
    {
        public static async Task SeedProductAsync(ProductContext productContext, ILogger logger)
        {
            if(!productContext.Products.Any())
            {
                productContext.AddRange(getCatalogProduct());
                await productContext.SaveChangesAsync();
                logger.Information("Seeded data for Product DB associated with context {DbContextName}", 
                    nameof(ProductContext));
            }
        }

        private static IEnumerable<CatalogProduct> getCatalogProduct()
        {
            return new List<CatalogProduct>()
            {
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    No = "Lotus",
                    Name = "Esprit",
                    Summary = "Sản phẩm chất lượng",
                    Description = "Sản phẩm có chất lượng tốt được bảo hành 1 năm 1 đổi 1 trong vòng 12 tháng dầu tiên kể từ ngày mua hàng",
                    Price = (decimal)177940.50,
                    StockQuantity = 100
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    No = "Lotus",
                    Name = "Esprit",
                    Summary = "Sản phẩm chất lượng",
                    Description = "Sản phẩm có chất lượng tốt được bảo hành 1 năm 1 đổi 1 trong vòng 12 tháng dầu tiên kể từ ngày mua hàng",
                    Price = (decimal)177940.50,
                    StockQuantity = 98
                }
            };
        }
    }
}
