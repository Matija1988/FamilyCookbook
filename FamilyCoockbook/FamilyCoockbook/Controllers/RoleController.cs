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
                return NotFound(response.Message);
            }

            return Ok(response);
        }
        
    }
}
