using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Mvc;
using static FamilyCookbook.REST_Models.Auth.AuthModels;

namespace FamilyCookbook.Controllers
{
    [ApiController]
    [Route("api/v0/category")]
    public class AuthController(IMemberService memberService) : ControllerBase
    {
        private readonly IMemberService _memberService = memberService;

        [HttpPost("token")]
        public async Task<IActionResult> UserLogIn(AuthLogIn logIn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _memberService.LogIn(logIn.username, logIn.password);

            if (response is not OkObjectResult) {
                return BadRequest(response.ToString());
            }

            return Ok(response);
        }


    }
}
