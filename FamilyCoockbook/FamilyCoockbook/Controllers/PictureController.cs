using FamilyCookbook.Mapping;
using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Picture;
using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Mvc;

namespace FamilyCookbook.Controllers
{
    [ApiController]
    [Route("api/v0/picture")]
    public class PictureController : ControllerBase
    {
        private readonly IService<Picture> _service;

        public PictureController(IService<Picture> service)
        {
            _service = service;
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
        public async Task<IActionResult> PostAsync(PictureCreate entity)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var mapper = new PictureMapping();

            var picture = mapper.PictureCreateToPicture(entity);

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
