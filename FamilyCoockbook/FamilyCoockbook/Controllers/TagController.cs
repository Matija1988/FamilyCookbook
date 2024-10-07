using FamilyCookbook.Common;
using FamilyCookbook.Mapping;
using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Tags;
using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace FamilyCookbook.Controllers
{
    [ApiController]
    [Route("api/v0/tag")]
    public sealed class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _tagService.GetAllAsync();

            return response.Success ? Ok(response) : BadRequest(response.Message.ToString());
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(List<TagCreate> entites)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var mapper = new TagMapper();

            var tags = mapper.ListTagToListTagCreate(entites);

            var response = await _tagService.CreateAsync(tags);
 
            if(response.IsSuccess == false)
            {
                return BadRequest(response.Message.ToString());
            }

            return Ok(response.Message.ToString());
            
        }

        [HttpPost]
        [Route("connectRecipeAndTags")]

        public async Task<IActionResult> ConnectRecipeAndTags(RecipeTagArray dto)
        {
            var response = await _tagService.ConnectRecipeAndTag(dto);

            if(response.IsSuccess == false)
            {
                return BadRequest(response.Message.ToString());
            }

            return Ok(response.Message.ToString());
        }

        [HttpGet]
        [Route("getBytext/{text}")]
        public async Task<IActionResult> GetByTextAsync(string text)
        {
            var response = await _tagService.GetByTextAsync(text);

            if(response.Success == false)
            {
                return NotFound(response.Message.ToString());
            }

            return Ok(response.Items);

        }

        [HttpGet]
        [Route("tags")]

        public async Task<IActionResult> PaginateAsync([FromQuery]Paging paging, [FromQuery]string? text)
        { 
            var response = await _tagService.PaginateAsync(paging, text);

            if(response.Success == false)
            {
                return NotFound(response.Message.ToString());
            }

            var finalResponse = new PaginatedList<ImmutableList<Tag>>();

            finalResponse.Items = response.Items;
            finalResponse.TotalCount = response.TotalCount;
            finalResponse.PageCount = response.PageCount;

            return Ok(finalResponse);
        }

        [HttpDelete]
        [Route("delete/{id:int}")]

        public async Task<IActionResult> DeleteAsync(int id)
        {
            var resposne = await _tagService.DeleteAsync(id);

            if(resposne.IsSuccess == false)
            {
                return BadRequest(resposne.Message.ToString());
            }

            return Ok(resposne.Message.ToString());
        }

        [HttpPut]
        [Route("update/{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, TagCreate tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mapper = new TagMapper();

            var entity = mapper.TagCreateToTag(tag);

            var response = await _tagService.UpdateAsync(id, entity);

            if(response.IsSuccess == false)
            {
                return BadRequest(response.Message.ToString());
            }

            return Ok(response.Message.ToString());
        }


    }
}
