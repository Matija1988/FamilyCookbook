
using FamilyCookbook.Common;
using FamilyCookbook.Mapping;
using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Category;
using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FamilyCookbook.Controllers
{
    [ApiController]
    [Route("api/v0/category")]
    public class CategoryController(ICategoryService service) : ControllerBase
    {
        private readonly ICategoryService _service = service;


        [Authorize(Roles = "Admin, Moderator, Contributor")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _service.GetAllAsync();

            if (response.Success == false)
            {
                return NotFound(response.Message.ToString());
            }

            return Ok(response);
        }


        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id) 
        { 
            var response = await _service.GetByIdAsync(id);

            if (response.Success == false) 
            {
                return NotFound(response.Message.ToString());            
            }
            return Ok(response.Items);
        }


        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [Route("create")]

        public async Task<IActionResult> CreateAsync(CategoryCreate entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = new Category();

            var mapper = new CategoryMapper();
            category = mapper.CategoryCreateToCategory(entity);

            var response = await _service.CreateAsync(category);

            if (response.Success == false)
            {
                return BadRequest(response.Message.ToString());     
            }
            return Ok(response);
        }


        [Authorize(Roles = "Admin, Moderator")]
        [HttpPut]
        [Route("update/{id:int}")]

        public async Task<IActionResult> UpdateAsync(int id, CategoryCreate entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = new Category();

            var mapper = new CategoryMapper();
            category = mapper.CategoryCreateToCategory(entity);


            var response = await _service.UpdateAsync(id, category);

            if(response.Success == false)
            {
                return BadRequest(response.Message.ToString());
            }
            return Ok(response.Message.ToString());
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("delete/{id:int}")]

        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await _service.DeleteAsync(id);

            if(response.Success==false)
            {
                return BadRequest(response.Message.ToString());
            }
            return Ok(response.Message.ToString());

        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("softDelete/{id:int}")]
        public async Task<IActionResult> SoftDeleteAsync(int id)
        {
            var response = await _service.SoftDeleteAsync(id);

            if(response.Success == false)
            {
                return BadRequest(response.Message.ToString());
            }
            return Ok(response.Message.ToString());
        }
    }
}
