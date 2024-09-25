using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Mvc;

namespace FamilyCookbook.Controllers
{
    [ApiController]
    [Route("api/v0/comment")]
    public sealed class CommentController(ICommentService commentService) : ControllerBase
    {
        private readonly ICommentService _commentService = commentService;

        [HttpGet]

        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _commentService.GetAllAsync();

            if (response.Success)
            {
                return Ok(response);
            }
            
            return BadRequest(response);
        }
    }
}
