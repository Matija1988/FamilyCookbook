
using FamilyCookbook.Common;
using FamilyCookbook.Common.Filters;
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
    public sealed class CategoryController : AbstractController<Category, CategoryRead, CategoryCreate, CategoryFilter>
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

            var category = _mapper.MapToEntity(entity);

            var response = await _service.CreateAsync(category);

            if (response.IsSuccess == false)
            {
                return BadRequest(response.Message.ToString());     
            }
            return Ok(response.Message.ToString());
        }

        [HttpGet]
        [Route("paging")]
        public async Task<IActionResult> PaginateAsync([FromQuery]Paging paging, [FromQuery]CategoryFilter filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _service.PaginateAsync(paging, filter);

            if(!response.Success)
            {
                return BadRequest(response.Message.ToString());
            }

            var finalResponse = new PaginatedList<List<CategoryRead>>();

            finalResponse.Items = _mapper.MapToReadList(response.Items.Value);
            finalResponse.PageCount = response.PageCount;
            finalResponse.TotalCount = response.TotalCount;

            return Ok(finalResponse);
        }

    }
}
