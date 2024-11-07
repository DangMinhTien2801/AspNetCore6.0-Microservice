using Contracts.Common.Interfaces;
using Product.Api.Entities;
using Product.Api.Persistence;

namespace Product.Api.Reponsitories.Interfaces
{
    public interface IProductReponsitory : 
        IRepositoryBaseAsync<CatalogProduct, string, ProductContext>
    {
        Task<IEnumerable<CatalogProduct>> GetProducts();
        Task<CatalogProduct?> GetProduct(string id);
        Task<CatalogProduct?> GetProductByNo(string productNo);
        Task CreateProduct(CatalogProduct product);
        Task UpdateProduct(CatalogProduct product);
        Task DeleteProduct(string id);
    }
}
