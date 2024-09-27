using FamilyCookbook.Mapping;
using FamilyCookbook.Model;
using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Mvc;
using static FamilyCookbook.REST_Models.Comment.CommentModels;

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
                return BadRequest(response.Message.ToString());
            }

            var mapper = new CommentMapper();

            var comments = mapper.CommentsReadList(response.Items);

            return Ok(comments);

        }

        [HttpGet]
        [Route("{id:int}")]

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

        [HttpPost]
        [Route("create")]

        public async Task<IActionResult> CreateAsync(CommentCreate newComment)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mapper = new CommentMapper();

            var comment = mapper.CommentCreate(newComment);

            comment.MemberId = newComment.memberId;
            comment.RecipeId = newComment.recipeId; 
            comment.Text = newComment.text;
            comment.Rating = newComment.rating;

            var response = await _commentService.CreateAsync(comment);

            if (response.Success == false) 
            { 
                return BadRequest(response.Message.ToString());
            }

            return Ok(response.Message.ToString());


        }
    }
}
