﻿using FamilyCookbook.Common;
using FamilyCookbook.Mapping;
using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Recipe;
using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace FamilyCookbook.Controllers
{

    [ApiController]
    [Route("api/v0/recipe")]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _service;

        private readonly IService<Category> _categoryService;

        public RecipeController(IRecipeService service, IMemberService memberService, IService<Category> categoryService)
        {
            _service = service;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _service.GetAllAsync();

            if (response.Success == false)
            {
                return NotFound(response.Message);
            }

            var mapper = new RecipeMapper();

            var recipes = mapper.RecipeToRecipeReadList(response.Items);

            return Ok(recipes);

        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _service.GetByIdAsync(id);

            if (response.Success == false)
            {
                return NotFound(response.Message);
            }
            var mapper = new RecipeMapper();

            var recipe = mapper.RecipeToRecipeRead(response.Items);

            return Ok(recipe);

        }

        [HttpGet]
        [Route("paginate")]

        public async Task<IActionResult> PaginateAsync([FromQuery]Paging paging)
        {
            var response = await _service.PaginateAsync(paging);

            if(response.Success == false)
            {
                return NotFound(response.Message);
            }
            var mapper = new RecipeMapper();

            var recipes = mapper.RecipeToRecipeReadList(response.Items);

            var finalResponse = new PaginatedList<List<RecipeRead>>();

            finalResponse.Items = recipes;
            finalResponse.TotalCount = response.TotalCount;
            finalResponse.PageCount = response.PageCount;

            return Ok(finalResponse);

        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateAsync(RecipeCreate newRecipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mapper = new RecipeMapper();

            var recipe = mapper.RecipeCreateToRecipe(newRecipe);

            var response = await _service.CreateAsync(recipe);

            if (response.Success == false)
            {
                return BadRequest(response.Message);
            }

            foreach (var item in newRecipe.MemberIds) 
            {
                var memberRecipe = new MemberRecipe();

                memberRecipe.RecipeId = response.Items.Id;
                memberRecipe.MemberId = item;

                var addMemberToRecipe = await _service.AddMemberToRecipe(memberRecipe);
            }

            var category = await _categoryService.GetByIdAsync(recipe.CategoryId);

            var returnRecipe = new RecipeRead();

            returnRecipe.Id = response.Items.Id;
            returnRecipe.CategoryName = category.Items.Name;
            returnRecipe.Title = response.Items.Title;
            returnRecipe.Subtitle = response.Items.Subtitle;
            returnRecipe.Text = response.Items.Text;
            
            
            return Ok(returnRecipe);

        }

        [HttpPut]
        [Route("update/{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, RecipeCreate updatedRecipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mapper = new RecipeMapper();

            var recipe = mapper.RecipeCreateToRecipe(updatedRecipe);

            var response = await _service.UpdateAsync(id, recipe);

            if (response.Success == false)
            {
                return BadRequest(response.Message);
            }
            return Ok(recipe);

        }

        [HttpPut]
        [Route("disable/{id:int}")]

        public async Task<IActionResult> SoftDeleteAsync(int id)
        {
            var response = await _service.SoftDeleteAsync(id);

            if(response.Success == false)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);

        }

        [HttpPost]
        [Route("AddMemberToRecipe")]
        public async Task<IActionResult> AddMemberToRecipe(MemberRecipe entity)
        {
            var response = await _service.AddMemberToRecipe(entity);

            if (response.Success == false) 
            {
                return BadRequest(response.Message);
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("RemoveMemberFromRecipe/{memberId:int}/{recipeId:int}")]
        public async Task<IActionResult> RemoveMemberFromRecipeAsync(int memberId, int recipeId)
        {
            var response = await _service.RemoveMemberFromRecipeAsync(memberId, recipeId);

            if(response.Success == false)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Message);
        }

    }
}