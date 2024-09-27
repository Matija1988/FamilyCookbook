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
            
        }


        //[HttpGet]
        //[Route("{id:int}")]

        //public async Task<IActionResult> GetById(int id)
        //{
        //    var response = await _commentService.GetByIdAsync(id);

        //    if(!response.Success)
        //    {
        //        return NotFound(response.Message.ToString());
        //    }

        //    var mapper = new CommentMapper();

        //    var comment = mapper.CommentRead(response.Items);

        //    return Ok(comment);
        //}

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


        [HttpPut]
        [Route("update/{id:int}")]

        public async Task<IActionResult> UpdateAsync(CommentCreate newComment, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mapper = new CommentMapper();

            var comment = mapper.CommentCreate(newComment);

            comment.MemberId = newComment.memberId;
            comment.RecipeId = newComment.recipeId;
            comment.Text = newComment.text;
            comment.Rating = newComment.rating;

            var response = await _commentService.UpdateAsync(id, comment);

            if (response.Success == false)
            {
                return BadRequest(response.Message.ToString());
            }

            return Ok(response.Message.ToString());

        }

        [HttpPut]
        [Route("softDelete/{id:int}")]
        public async Task<IActionResult> SoftDeleteAsync(int id)
        {
            var response = await _commentService.SoftDeleteAsync(id);

            if (response.Success == false) 
            {
                return BadRequest(response.Message.ToString());
            }

            return Ok(response.Message.ToString());
        }

        [HttpDelete]
        [Route("delete/{id:int}")]
        public async Task<IActionResult> PermaDeleteAsync(int id)
        {
            var response = await _commentService.DeleteAsync(id);

            if (response.Success == false)
            {
                return BadRequest(response.Message.ToString());
            }
            return Ok(response.Message.ToString());
            
        }
    }
}
