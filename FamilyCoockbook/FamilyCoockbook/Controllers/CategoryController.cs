
using FamilyCookbook.Mapping;
using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Category;
using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Mvc;

namespace FamilyCookbook.Controllers
{
    [ApiController]
    [Route("api/v0/category")]
    public class CategoryController : ControllerBase
    {
        private readonly IService<Category> _service;

        public CategoryController(IService<Category> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _service.GetAllAsync();

            if (response.Success == false)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id) 
        { 
            var response = await _service.GetByIdAsync(id);

            if (response.Success == false) 
            {
                return NotFound(response.Message);            
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("create")]

        public async Task<IActionResult> CreateAsync(CategoryCreate entity)
        {
            var category = new Category();

            var mapper = new CategoryMapper();
            category = mapper.CategoryCreateToCategory(entity);

            var response = await _service.CreateAsync(category);

            if (response.Success == false)
            {
                return BadRequest(response.Message);     
            }
            return Ok(response);
        }

        [HttpPut]
        [Route("update/{id:int}")]

        public async Task<IActionResult> UpdateAsync(int id, Category entity)
        {
            var response = await _service.UpdateAsync(id, entity);

            if(response.Success == false)
            {
                return BadRequest(response.Message);
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("delete/{id:int}")]

        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await _service.DeleteAsync(id);

            if(response.Success==false)
            {
                return BadRequest(response.Message);
            }
            return Ok(response);

        }

    }

}
