using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Product.Api.Entities;
using Product.Api.Persistence;
using Product.Api.Reponsitories.Interfaces;

namespace Product.Api.Reponsitories
{
    public class ProductRepositoty :
        RepositoryBaseAsync<CatalogProduct, string, ProductContext>, IProductReponsitory
    {
        public ProductRepositoty(ProductContext dbContext, IUnitOfWork<ProductContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public async Task CreateProduct(CatalogProduct product)
        {
            await CreateAsync(product);
        }

        public async Task DeleteProduct(string id)
        {
            var product = await GetByIdAsync(id);
            if(product != null)
                await DeleteAsync(product);
        }

        public async Task<CatalogProduct?> GetProduct(string id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<CatalogProduct?> GetProductByNo(string productNo)
        {
            return await FindByCondition(x => x.No == productNo).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CatalogProduct>> GetProducts()
        {
            return await FindAll().ToListAsync();
        }

        public async Task UpdateProduct(CatalogProduct product)
        {
            await UpdateAsync(product);
        }
    }
}
