namespace FamilyCookbook.Service.Common
{
    public interface ICommentService : IService<Comment, CommentFilter>
    {
        Task<MessageResponse> CreateAsync(Comment comment);

        Task<RepositoryResponse<List<Comment>>> GetRecipeCommentsAsync(int recipeId);
    }
}
