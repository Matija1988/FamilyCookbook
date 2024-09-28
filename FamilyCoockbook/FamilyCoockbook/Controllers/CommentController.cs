using Autofac.Core;
using FamilyCookbook.Mapping;
using FamilyCookbook.Model;
using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Mvc;
using static FamilyCookbook.REST_Models.Comment.CommentModels;

namespace FamilyCookbook.Controllers
{
    [ApiController]
    [Route("api/v0/comment")]
    public sealed class CommentController : AbstractController<Comment, CommentRead, CommentCreate>
    {
        private readonly ICommentService _commentService;
        private readonly IMapper<Comment, CommentRead, CommentCreate> _mapper;

        public CommentController(
            ICommentService service, IMapper<Comment, CommentRead, CommentCreate> mapper) 
            : base (service, mapper)
        {
            _commentService = service;   
        }

        [HttpPost]
        [Route("create")]

        public async Task<IActionResult> CreateAsync(CommentCreate newComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mapper = new CommentMapper();

            var comment = mapper.CommentCreate(newComment);

            var response = await _commentService.CreateAsync(comment);

            if (response.Success == false)
            {
                return BadRequest(response.Message.ToString());
            }

            return Ok(response.Message.ToString());

        }

    }
}
