namespace MyAPISolution.SampleAPI.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public CategoryDTO? Category { get; set; }  
    }
}
