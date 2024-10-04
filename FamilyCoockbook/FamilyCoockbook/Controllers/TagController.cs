﻿using FamilyCookbook.Mapping;
using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Tags;
using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FamilyCookbook.Controllers
{
    [ApiController]
    [Route("api/v0/tag")]
    public class TagController : ControllerBase
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

            return response.Success ? Ok(response) : BadRequest(response.Message);
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
 
            if(response.Success == false)
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

    }
}
