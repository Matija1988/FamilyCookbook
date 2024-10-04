
using FamilyCookbook.Common;
using FamilyCookbook.Mapping;
using FamilyCookbook.Mapping.MapperWrappers;
using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Category;
using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FamilyCookbook.Controllers
{
    [ApiController]
    [Route("api/v0/category")]
    public sealed class CategoryController : AbstractController<Category, CategoryRead, CategoryCreate>
    {
        private readonly ICategoryService _service;
        private readonly IMapper<Category, CategoryRead, CategoryCreate> _mapper;

        public CategoryController(ICategoryService service, 
            IMapper<Category, CategoryRead, CategoryCreate> mapper) 
            : base(service, mapper)
        {
            _service = service;
            _mapper = mapper;
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
            return Ok(response.Message.ToString());
        }

    }
}
