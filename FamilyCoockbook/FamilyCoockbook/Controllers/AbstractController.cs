﻿using FamilyCookbook.Mapping.MapperWrappers;
using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Category;
using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Riok.Mapperly.Abstractions;

namespace FamilyCookbook.Controllers
{
    public class AbstractController<T, TDR, TDI, Filter> : ControllerBase where T : class
    {
        protected readonly IService<T, Filter> _service;

        protected readonly IMapper<T, TDR, TDI> _mapper;
        private ICategoryService service;
        private IMapper<Category, CategoryRead, CategoryCreate> mapper;

        public AbstractController(IService<T, Filter> service, IMapper<T, TDR, TDI> mapper)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _service.GetAllAsync();

            if(response.Success == false)
            {
                return NotFound(response.Message.ToString());
            }

            var entities = _mapper.MapToReadList(response.Items);

            var finalResponse = new PaginatedList<List<TDR>>();

            finalResponse.Items = entities;
            finalResponse.TotalCount = response.TotalCount;
            finalResponse.PageCount = response.PageCount;

            return Ok(finalResponse);

        }

        [HttpGet]
        [Route("{id:int}")]

        public async Task<IActionResult> GetById(int id)
        {
            var response = await _service.GetByIdAsync(id);

            if(response.Success == false)
            {
                return NotFound(response.Message.ToString());
            }

            var entity = _mapper.MapReadToDto(response.Items);

            return Ok(entity);

        }

        [HttpPut]
        [Route("update/{id:int}")]

        public async Task<IActionResult> PutAsync(TDI dto, int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = _mapper.MapToEntity(dto);

            var response = await _service.UpdateAsync(id, entity);

            if (response.IsSuccess == false) 
            {
                return BadRequest(response.Message.ToString());
            }

            return Ok(response.Message.ToString());
            
        }

        [Authorize(Roles ="Admin, Moderator")]
        [HttpPut]
        [Route("softDelete/{id:int}")]
        public async Task<IActionResult> SoftDeleteAsync(int id)
        {
            var response = await _service.SoftDeleteAsync(id);

            if (response.Success == false)
            {
                return BadRequest(response.Message.ToString());
            }

            return Ok(response.Message.ToString());
        }

        [Authorize("Admin")]
        [HttpDelete]
        [Route("delete/{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await _service.DeleteAsync(id);

            if(response.IsSuccess == false)
            {
                return BadRequest(response.Message.ToString());
            }

            return Ok(response.Message.ToString());
        }

    }
}
