using FamilyCookbook.Common;
using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Mvc;

namespace FamilyCookbook.Controllers
{

    [ApiController]
    [Route("api/v0/role")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        
        public async Task<IActionResult> GetAll()
        {
            
            var response = await _roleService.GetAllAsync();

            if(response.Success == false)
            {
                return NotFound(response.Message.ToString());
            }

            return Ok(response.Items);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _roleService.GetByIdAsync(id);

            if (response.Success == false) 
            {
                return NotFound(response.Message.ToString());
            }
            return Ok(response.Items);
        } 
        
    }
}
