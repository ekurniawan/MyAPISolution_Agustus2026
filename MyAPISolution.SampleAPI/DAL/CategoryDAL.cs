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
            catch (Exception ex)
            {
                throw new Exception("Error retrieving all categories", ex);
            }
        }

        public async Task<Category> GetCategoryById(int id)
        {
            try
            {
                var result = await _rapidDbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
                
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving category by ID", ex);
            }
        }

        public Task<Category> UpdateCategory(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
