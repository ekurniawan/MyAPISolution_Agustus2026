using MyAPISolution.SampleAPI.Models;

namespace MyAPISolution.SampleAPI.DAL
{
    public class CategoryMockDAL : ICategoryDAL
    {
        List<Category> categories = new List<Category>()
        {
            new Category() { CategoryId = 1, CategoryName = "Beverages" },
            new Category() { CategoryId = 2, CategoryName = "Condiments" },
            new Category() { CategoryId = 3, CategoryName = "Confections" },
            new Category() { CategoryId = 4, CategoryName = "Dairy Products" },
            new Category() { CategoryId = 5, CategoryName = "Grains/Cereals" },
            new Category() { CategoryId = 6, CategoryName = "Meat/Poultry" },
            new Category() { CategoryId = 7, CategoryName = "Produce" },
            new Category() { CategoryId = 8, CategoryName = "Seafood" }
        };

        public CategoryMockDAL() { 
        
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
           return await Task.FromResult(categories);
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
