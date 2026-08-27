using Microsoft.AspNetCore.Mvc;
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

        public async Task<Category> CreateCategory(Category category)
        {
            try
            {
                _rapidDbContext.Categories.Add(category);
                await _rapidDbContext.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating category", ex);
            }
        }

        public async Task<Category> DeleteCategory(int id)
        {
            try
            {
                var category = await _rapidDbContext.Categories.FindAsync(id);
                if (category == null)
                {
                    throw new Exception("Category not found");
                }
                _rapidDbContext.Categories.Remove(category);
                await _rapidDbContext.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting category", ex);
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            try
            {
                var result = await _rapidDbContext.Categories.OrderBy(c=>c.CategoryName).AsNoTracking().ToListAsync();
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
                var result = await _rapidDbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.CategoryId == id);
                
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving category by ID", ex);
            }
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            try
            {
                _rapidDbContext.Categories.Update(category);
                await _rapidDbContext.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating category", ex);
            }
        }
    }
}
