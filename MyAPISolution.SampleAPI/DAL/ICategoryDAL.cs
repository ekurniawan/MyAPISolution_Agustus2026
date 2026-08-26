using MyAPISolution.SampleAPI.Models;

namespace MyAPISolution.SampleAPI.DAL
{
    public interface ICategoryDAL
    {
        //crud
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> GetCategoryById(int id);
        Task<Category> CreateCategory(Category category);
        Task<Category> UpdateCategory(Category category);
        Task<Category> DeleteCategory(int id);
    }
}
