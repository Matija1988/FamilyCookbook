using Autofac.Core;
using FamilyCookbook.Model;
using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Mvc;

namespace FamilyCookbook.Controllers
{
    [ApiController]
    [Route("api/v0/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IMemberService _service;
        public RegisterController(IMemberService memberService)
        {
            _service = memberService;
        }

        [HttpPost]
        [Route("registerUser")]

        public async Task<IActionResult> RegisterUser(UserRegistry user)
        {
            var response = await _service.UserRegister(user);

            if (response.IsSuccess == false) { return BadRequest(response.Message.ToString()); }

            return Ok(response.Message.ToString());
        }


    }
}
