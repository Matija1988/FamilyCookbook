using FamilyCookbook.Mapping;
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

            if (!response.Success)
            {
                return BadRequest(response);
            }

            var mapper = new CommentMapper();

            var comments = mapper.CommentsReadList(response.Items);

            return Ok(comments);

        }

        [HttpGet]
        [Route("[controller]/{id:int}")]

        public async Task<IActionResult> GetById(int id)
        {
            var response = await _commentService.GetByIdAsync(id);

            if(!response.Success)
            {
                return NotFound(response.Message.ToString());
            }

            var mapper = new CommentMapper();

            var comment = mapper.CommentRead(response.Items);

            return Ok(comment);
        }
    }
}
