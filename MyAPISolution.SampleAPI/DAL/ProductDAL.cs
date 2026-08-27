using Microsoft.EntityFrameworkCore;
using MyAPISolution.SampleAPI.Models;

namespace MyAPISolution.SampleAPI.DAL
{
    public class ProductDAL : IProductDAL
    {
        private readonly RapidDbContext _rapidDbContext;

        public ProductDAL(RapidDbContext rapidDbContext)
        {
            _rapidDbContext = rapidDbContext;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var result = await _rapidDbContext.Products.Include(p => p.Category).OrderBy(p => p.ProductName).AsNoTracking().ToListAsync();
            return result;
        }

        public async Task<Product> GetProductById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
