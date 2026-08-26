using Microsoft.AspNetCore.Mvc;
using MyAPISolution.SampleAPI.DAL;
using MyAPISolution.SampleAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyAPISolution.SampleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryDAL _categoryDAL;

        public CategoriesController(ICategoryDAL categoryDAL)
        {
            _categoryDAL = categoryDAL;
        }

        // GET: api/<CategoriesController>
        [HttpGet]
        public async Task<IEnumerable<Category>> Get()
        {
            return await _categoryDAL.GetAllCategories();
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
