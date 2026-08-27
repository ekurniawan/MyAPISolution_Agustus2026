using System.ComponentModel.DataAnnotations;

namespace MyAPISolution.SampleAPI.DTO
{
    public class CategoryInsertDTO
    {
        [Required(ErrorMessage = "CategoryName is required.")]
        public string CategoryName { get; set; } = string.Empty;
    }
}
