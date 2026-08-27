using Microsoft.AspNetCore.Mvc;
using MyAPISolution.SampleAPI.DAL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyAPISolution.SampleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductDAL _productDAL;

        public ProductsController(IProductDAL productDAL)
        {
            _productDAL = productDAL;
        }

        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _productDAL.GetAllProducts();
            var productDTO = products.Select(p => new
            {
                p.ProductId,
                p.ProductName,
                p.Price,
                Category = new
                {
                    p.Category.CategoryId,
                    p.Category.CategoryName
                }
            });

            return Ok(productDTO);
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
