using System.ComponentModel.DataAnnotations;

namespace MyAPISolution.SampleAPI.DTO
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
