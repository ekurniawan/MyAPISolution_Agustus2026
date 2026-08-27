using System.ComponentModel.DataAnnotations;

namespace MyAPISolution.SampleAPI.DTO
{
    public class CategoryEditDTO
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "CategoryName is required.")]
        public string CategoryName { get; set; } = string.Empty;
    }
}
