﻿using FamilyCookbook.Mapping;
using FamilyCookbook.Model;
using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Mvc;
using Riok.Mapperly.Abstractions;

namespace FamilyCookbook.Controllers
{
    public class AbstractController<T, TDR, TDI> : ControllerBase where T : class
    {
        protected readonly IService<T> _service;

        protected readonly IMapper<T, TDR, TDI> _mapper;

        public AbstractController(IService<T> service, IMapper<T, TDR, TDI> mapper)
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

       
    }
}
