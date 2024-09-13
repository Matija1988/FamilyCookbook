﻿using FamilyCookbook.Common;
using FamilyCookbook.Common.Upload;
using FamilyCookbook.Mapping;
using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Picture;
using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Mvc;

namespace FamilyCookbook.Controllers
{
    [ApiController]
    [Route("api/v0/[controller]")]
    public class PictureController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IPictureService _service;
        private readonly IRecipeService _recipeService;
        public PictureController(IWebHostEnvironment environment,
            IPictureService service, 
            IRecipeService recipeService)
        {
            _service = service;
            _environment = environment; 
            _recipeService = recipeService;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _service.GetAllAsync();

            if (response.Success == false)
            {
                return NotFound(response.Message);
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
                return NotFound(response.Message);
            }
            return Ok(response);

        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> PostAsync(IFormFile file,  string name)
        {
            if(!ModelState.IsValid || file ==  null)
            {
                return BadRequest(ModelState);
            }

            var sizeValidation = PictureUpload.ValidatePictureSize(file);
            if(sizeValidation is not OkResult)
            {
                return sizeValidation;
            }

            var extensionValidation = PictureUpload.ValidateFileExtension(file);
            if (extensionValidation is not OkResult) 
            { 
                return extensionValidation;
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);
            var relativePath = Path.Combine("uploads", fileName);

            await PictureUpload.SavePictureAsync(file, filePath);

            var mapper = new PictureMapping();

            PictureCreate pictureDTO = new PictureCreate { Name = name };

            var picture = mapper.PictureCreateToPicture(pictureDTO);

            picture.Location = relativePath;
            

            var response = await _service.CreateAsync(picture);

            if (response.Success == false)
            {
                return BadRequest(response.Message);
            }
            return Ok(response);

        }
        [HttpPut]
        [Route("update/{id:int}")]
        public async Task<IActionResult> PutAsync(int id, PictureCreate entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mapper = new PictureMapping();
            var picture = mapper.PictureCreateToPicture(entity);

            var response = await _service.UpdateAsync(id, picture);

            if(response.Success == false)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);

        }

        [HttpDelete]
        [Route("delete/{id:int}")] 
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await _service.DeleteAsync(id);

            if(response.Success == false)
            {
                return NotFound(response.Message);
            }

            return Ok(response.Message);

        }
    }
}
