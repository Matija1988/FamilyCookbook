using AngleSharp.Dom;
using FamilyCookbook.Common;
using FamilyCookbook.Common.Upload;
using FamilyCookbook.Common.Validations;
using FamilyCookbook.Mapping;
using FamilyCookbook.Mapping.MapperWrappers;
using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Recipe;
using FamilyCookbook.Service.Common;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Reflection.Metadata.Ecma335;

namespace FamilyCookbook.Controllers
{

    [ApiController]
    [Route("api/v0/recipe")]
    public class RecipeController(IWebHostEnvironment environment,
        IRecipeService service,
        ICategoryService categoryService,
        IPictureService pictureService,
        IMapperExtended<Recipe, RecipeRead, RecipeCreate, RecipeCreateDTO> mapper) : ControllerBase
    {
        private readonly IRecipeService _service = service;
        private readonly IWebHostEnvironment _enviroment = environment;
        private readonly ICategoryService _categoryService = categoryService;
        private readonly IPictureService _pictureService = pictureService;
        private readonly IMapperExtended<Recipe, RecipeRead, RecipeCreate, RecipeCreateDTO> _mapper = mapper;

        [Authorize, Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _service.GetAllAsync();

            if (response.Success == false)
            {
                return NotFound(response.Message.ToString());
            }

            var recipes = _mapper.MapToReadList(response.Items);

            return Ok(recipes);

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

            var recipe = _mapper.MapReadToDto(response.Items);

            return Ok(recipe);

        }

        [HttpGet]
        [Route("paginate")]

        public async Task<IActionResult> PaginateAsync([FromQuery] Paging paging, [FromQuery] RecipeFilter filter)
        {
            var response = await _service.PaginateAsync(paging, filter);

            if (response.Success == false)
            {
                return NotFound(response.Message.ToString());
            }
          
            var recipes = _mapper.MapToReadList(response.Items);

            var finalResponse = new PaginatedList<List<RecipeRead>>();

            finalResponse.Items = recipes;
            finalResponse.TotalCount = response.TotalCount;
            finalResponse.PageCount = response.PageCount;

            return Ok(finalResponse);

        }

        [Authorize(Roles = "Admin, Moderator, Contributor")]
        [HttpGet]
        [Route("getRecipesWithoutAuthors")]
        public async Task<IActionResult> GetRecipesWithoutAuthors()
        {
            var response = await _service.GetRecipesWithoutAuthors();

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response.Message.ToString());
        }


        [Authorize(Roles = "Admin, Moderator, Contributor")]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateAsync(RecipeCreate newRecipe)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!PictureUpload.ValidatePictureSizeFunc
                (string.IsNullOrEmpty(newRecipe.PictureBlob), newRecipe.PictureBlob))
            {
                return BadRequest("Invalid picture size. Keep the images under 1MB");
            }

            byte[] imageBytes = null;
            string fileExtension = "";

            if (!string.IsNullOrEmpty(newRecipe.PictureBlob))
            {
                var dataParts = PictureUpload.Base64DataParts(newRecipe.PictureBlob);
                var mimeType = PictureUpload.GetMimeType(dataParts, 0);                    
                fileExtension = PictureUpload.ValidateFileExtensionFunc(mimeType);
                imageBytes = PictureUpload.ConvertBase64ToByteArray(dataParts, 1);

            }

            string relativePath = "";

            var uploadsFolder = Path.Combine(_enviroment.WebRootPath, "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var pictures = await _pictureService.GetAllAsync();

            var image = pictures.Items.Find(pic => pic.Name == newRecipe.PictureName);

            if (image is null && imageBytes != null)
            {
                var fileName = newRecipe.PictureName + fileExtension;
                var filePath = Path.Combine(uploadsFolder, fileName);
                relativePath = Path.Combine("uploads", fileName);

                await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);
            }
            else if (image is not null)
            {
                relativePath = image.Location;
            }

            var sanitizer = new HtmlSanitizer();

            string sanitizedText = sanitizer.Sanitize(newRecipe.Text);

            newRecipe.Text = sanitizedText;

            var recipe = _mapper.MapReadToCreateDTO(newRecipe);

            Picture interMediaryPicture = new Picture();

            interMediaryPicture.Name = newRecipe.PictureName;
            interMediaryPicture.Location = relativePath;

            recipe.Picture = interMediaryPicture;

            var response = await _service.CreateAsync(recipe);

            if (response.IsSuccess == false)
            {
                return BadRequest(response.Message.ToString());
            }

            return Ok(response.Message.ToString());

        }

        [Authorize(Roles = "Admin, Moderator, Contributor")]
        [HttpPut]
        [Route("addPictureToRecipe/{recipeId:int}/{pictureId:int}")]

        public async Task<IActionResult> AddPictureToRecipe(int recipeId, int pictureId)
        {
            var response = await _service.AddPictureToRecipeAsync(pictureId, recipeId);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response.Message.ToString());
        }

        [Authorize(Roles = "Admin, Moderator, Contributor")]
        [HttpPut]
        [Route("update/{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, RecipeCreate updatedRecipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            byte[] imageBytes = null;
            string fileExtension = "";
            string relativePath = "";

            try
            {
                if (!string.IsNullOrEmpty(updatedRecipe.PictureBlob))
                {
                    var base64DataParts = updatedRecipe.PictureBlob.Split(',');
                    var mimeType = base64DataParts[0];
                    var base64Data = base64DataParts[1];

                    imageBytes = Convert.FromBase64String(base64Data);

                    fileExtension = mimeType switch
                    {
                        "data:image/jpeg;base64" => ".jpg",
                        "data:image/jpg;base64" => ".jpg",
                        "data:image/png;base64" => ".png",
                        _ => ""
                    };

                    if (string.IsNullOrEmpty(fileExtension))
                    {
                        return BadRequest("Unsuported file type. Use JPEG, JPG or PNG!");
                    }

                }
            }

            catch (Exception ex)
            {
                return BadRequest($"Failed to create image: {ex.Message}");
            }

            var uploadsFolder = Path.Combine(_enviroment.WebRootPath, "uploads");

            if(!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var pictures = await _pictureService.GetAllAsync();

            var image = pictures.Items.Find(pic => pic.Name == updatedRecipe.PictureName);

            Picture intemediaryPicture = new Picture();

            if (image is null && imageBytes != null)
            {
                var fileName = updatedRecipe.PictureName + fileExtension;
                var filePath = Path.Combine(uploadsFolder, fileName);
                relativePath = Path.Combine("uploads", fileName);

                
                intemediaryPicture.Location = relativePath;
                intemediaryPicture.Name = updatedRecipe.PictureName;

                await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);
            }
            else if (image is not null) 
            { 
                relativePath = image.Location;

                intemediaryPicture.Id = image.Id;
                intemediaryPicture.Location = image.Location;
                intemediaryPicture.Name = updatedRecipe.PictureName;

            }

            var sanitizer = new HtmlSanitizer();

            string sanitizedText = sanitizer.Sanitize(updatedRecipe.Text);

            updatedRecipe.Text = sanitizedText;

            var recipe = _mapper.MapReadToCreateDTO(updatedRecipe);

            recipe.Picture = intemediaryPicture;

            var response = await _service.UpdateAsync(id, recipe);

            if (response.IsSuccess == false)
            {
                return BadRequest(response.Message.ToString());
            }

            return Ok(recipe);

        }


        [Authorize(Roles = "Admin, Moderator")]
        [HttpPut]
        [Route("disable/{id:int}")]

        public async Task<IActionResult> SoftDeleteAsync(int id)
        {
            var response = await _service.SoftDeleteAsync(id);

            if (response.Success == false)
            {
                return BadRequest(response.Message.ToString());
            }

            return Ok(response);

        }


        [Authorize(Roles = "Admin, Moderator, Contributor")]
        [HttpPost]
        [Route("AddMemberToRecipe")]
        public async Task<IActionResult> AddMemberToRecipe(MemberRecipe entity)
        {
            var response = await _service.AddMemberToRecipe(entity);

            if (response.Success == false)
            {
                return BadRequest(response.Message.ToString());
            }
            return Ok(response);
        }


        [Authorize(Roles = "Admin, Moderator")]
        [HttpDelete]
        [Route("RemoveMemberFromRecipe/{memberId:int}/{recipeId:int}")]
        public async Task<IActionResult> RemoveMemberFromRecipeAsync(int memberId, int recipeId)
        {
            var response = await _service.RemoveMemberFromRecipeAsync(memberId, recipeId);

            if (response.Success == false)
            {
                return BadRequest(response.Message.ToString());
            }
            return Ok(response.Message.ToString());
        }

    }
}
