using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Mvc;

namespace FamilyCookbook.Controllers
{
    [ApiController]
    [Route("api/v0/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
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

            return Ok(response);
        }
    }
}
