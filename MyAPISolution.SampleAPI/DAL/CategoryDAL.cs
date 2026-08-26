using Microsoft.EntityFrameworkCore;
using MyAPISolution.SampleAPI.Models;

namespace MyAPISolution.SampleAPI.DAL
{
    public class CategoryDAL : ICategoryDAL
    {
        private readonly RapidDbContext _rapidDbContext;
        public CategoryDAL(RapidDbContext rapidDbContext) 
        {
            _rapidDbContext = rapidDbContext;
        }

        public Task<Category> CreateCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public Task<Category> DeleteCategory(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            try
            {
                var result = await _rapidDbContext.Categories.OrderBy(c=>c.CategoryName).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<Category> GetCategoryById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Category> UpdateCategory(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
