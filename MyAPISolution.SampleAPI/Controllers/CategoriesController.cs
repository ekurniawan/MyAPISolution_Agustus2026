using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAPISolution.SampleAPI.DAL;
using MyAPISolution.SampleAPI.DTO;
using MyAPISolution.SampleAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyAPISolution.SampleAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryDAL _categoryDAL;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryDAL categoryDAL, IMapper mapper, ILogger<CategoriesController> logger)
        {
            _categoryDAL = categoryDAL;
            _mapper = mapper;
            _logger = logger;
        }
 
        // GET: api/<CategoriesController>
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                /*var categories = await _categoryDAL.GetAllCategories();
                List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();
                foreach (var category in categories)
                {
                    categoryDTOs.Add(new CategoryDTO
                    {
                        CategoryId = category.CategoryId,
                        CategoryName = category.CategoryName
                    });
                }

                return Ok(categoryDTOs);*/
                var categories = await _categoryDAL.GetAllCategories();
                var categoryDTOs = _mapper.Map<List<CategoryDTO>>(categories);

                // Example: manual business-level logging inside the controller action.
                // Because this logger's SourceContext is under "...Controllers", it is routed
                // to the same action log file (Logs/action-*.txt) as TransactionLoggingFilter entries.
                _logger.LogInformation("Retrieved {Count} categories", categoryDTOs.Count);

                return Ok(categoryDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to retrieve categories");
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Policy = "RequireAdminRole")]
        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                /*var category = await _categoryDAL.GetCategoryById(id);
               
                if (category == null)
                {
                    return NotFound();
                }

                CategoryDTO categoryDTO = new CategoryDTO
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName
                };

                return Ok(categoryDTO);*/
                var category = await _categoryDAL.GetCategoryById(id);
                if (category == null)
                {
                    return NotFound();
                }
                var categoryDTO = _mapper.Map<CategoryDTO>(category);
                return Ok(categoryDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryInsertDTO categoryInsertDTO)
        {
            try
            {
                if (ModelState.IsValid) {
                    /*var createdCategory = await _categoryDAL.CreateCategory(new Category
                    {
                        CategoryName = categoryInsertDTO.CategoryName
                    });
                    var createdCategoryDTO = new CategoryDTO
                    {
                        CategoryId = createdCategory.CategoryId,
                        CategoryName = createdCategory.CategoryName
                    };
                    return CreatedAtAction(nameof(Get), new { id = createdCategoryDTO.CategoryId }, createdCategoryDTO);*/
                    var category = _mapper.Map<Category>(categoryInsertDTO);
                    var createdCategory = await _categoryDAL.CreateCategory(category);
                    var createdCategoryDTO = _mapper.Map<CategoryDTO>(createdCategory);
                    return CreatedAtAction(nameof(Get), new { id = createdCategoryDTO.CategoryId }, createdCategoryDTO);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CategoryEditDTO categoryEditDTO)
        {
            try
            {
                /*var updatedCategory = await _categoryDAL.UpdateCategory(new Category
                {
                    CategoryId = id,
                    CategoryName = categoryEditDTO.CategoryName
                });
                if (updatedCategory == null)
                {
                    return NotFound();
                }
                var categoryDto = new CategoryDTO
                {
                    CategoryId = updatedCategory.CategoryId,
                    CategoryName = updatedCategory.CategoryName
                };
                return Ok(categoryDto);*/
                if (ModelState.IsValid)
                {
                    var category = _mapper.Map<Category>(categoryEditDTO);
                    var updateCategory = await _categoryDAL.UpdateCategory(category);
                    
                    if (updateCategory != null) { 
                        return NotFound();
                    }
                    var categoryDto = _mapper.Map<CategoryDTO>(categoryEditDTO);
                    return Ok(categoryDto);
                }
                else
                {
                    return BadRequest(ModelState);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deletedCategory = await _categoryDAL.DeleteCategory(id);
                if (deletedCategory == null)
                {
                    return NotFound();
                }
                return Ok(deletedCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
