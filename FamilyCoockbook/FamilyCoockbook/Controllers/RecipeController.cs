using FamilyCookbook.Common;
using FamilyCookbook.Common.Upload;
using FamilyCookbook.Common.Validations;
using FamilyCookbook.Mapping;
using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Recipe;
using FamilyCookbook.Service.Common;
using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace FamilyCookbook.Controllers
{

    [ApiController]
    [Route("api/v0/recipe")]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _service;
        private readonly IWebHostEnvironment _enviroment; 
        private readonly ICategoryService _categoryService;

        public RecipeController(IWebHostEnvironment environment, IRecipeService service, ICategoryService categoryService)
        {
            _service = service;
            _categoryService = categoryService;
            _enviroment = environment;
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

        public async Task<IActionResult> PaginateAsync([FromQuery]Paging paging, [FromQuery] RecipeFilter filter)
        {
            var response = await _service.PaginateAsync(paging, filter);

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

        [HttpGet]
        [Route("getRecipesWithoutAuthors")]

        public async Task<IActionResult> GetRecipesWithoutAuthors()
        {
            var response = await _service.GetRecipesWithoutAuthors();

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateAsync([FromForm] RecipeCreate newRecipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var validatePicture = PictureValidations.ValidatePicture(newRecipe.Picture);

            if(validatePicture is not OkResult)
            {
                return validatePicture;
            }

            var uploadsFolder = Path.Combine(_enviroment.WebRootPath, "uploads");
            
            if(!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(newRecipe.Picture.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);
            var relativePath = Path.Combine("uploads", fileName);

            await PictureUpload.SavePictureAsync(newRecipe.Picture, filePath);

            var sanitizer = new HtmlSanitizer();

            string sanitizedText = sanitizer.Sanitize(newRecipe.Text);

            newRecipe.Text = sanitizedText;

            var mapper = new RecipeMapper();

            var recipe = mapper.RecipeCreateToRecipeCreateDTO(newRecipe);

            recipe.Picture.Location = relativePath;
            recipe.Picture.Name = newRecipe.PictureName;

            var response = await _service.CreateAsync(recipe);

            if (response.Success == false)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Message);

        }

       

        [HttpPut]
        [Route("addPictureToRecipe/{recipeId:int}/{pictureId:int}")]

        public async Task<IActionResult> AddPictureToRecipe(int recipeId, int pictureId)
        {
            var response = await _service.AddPictureToRecipeAsync(pictureId, recipeId);

            if(response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);    
        }

        [HttpPut]
        [Route("update/{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, RecipeCreate updatedRecipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sanitizer = new HtmlSanitizer();

            string sanitizedText = sanitizer.Sanitize(updatedRecipe.Text);

            updatedRecipe.Text = sanitizedText;

            var mapper = new RecipeMapper();

            var recipe = mapper.RecipeCreateToRecipe(updatedRecipe);

            var response = await _service.UpdateAsync(id, recipe);

            if (response.Success == false)
            {
                return BadRequest(response.Message);
            }

            foreach (var item in updatedRecipe.MemberIds)
            {
                var memberRecipe = new MemberRecipe();

                memberRecipe.RecipeId = id;
                memberRecipe.MemberId = item;

                var addMemberToRecipe = await _service.AddMemberToRecipe(memberRecipe);
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
