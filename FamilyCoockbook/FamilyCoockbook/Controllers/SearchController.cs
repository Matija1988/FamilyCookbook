using FamilyCookbook.Mapping.MapperWrappers;
using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Recipe;
using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Mvc;

namespace FamilyCookbook.Controllers
{
    [ApiController]
    [Route("api/v0/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;
        private readonly IMapperExtended<Recipe, RecipeRead, RecipeCreate, RecipeCreateDTO> _mapper;
        public SearchController(ISearchService searchService, 
            IMapperExtended<Recipe, RecipeRead, RecipeCreate, RecipeCreateDTO> mapper)
        {
            _searchService = searchService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("search/")]

        public async Task<IActionResult> GetByText(string text)
        {
            var response = await _searchService.GetAllBySearchText(text);

            if(!response.Success)
            {
                return BadRequest(response.Message.ToString());
            }

            var recipes = _mapper.MapListToReadList(response.Items);

            return Ok(recipes);
        }
    }
}
