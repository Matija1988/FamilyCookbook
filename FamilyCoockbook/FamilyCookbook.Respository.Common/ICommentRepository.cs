namespace FamilyCookbook.Repository.Common
{
    public interface ICommentRepository : IRepository<Comment, CommentFilter>
    {
        Task<MessageResponse> CreateAsync(Comment comment);
        Task<RepositoryResponse<List<Comment>>> GetRecipeCommentsAsync(int recipeId);

    }
}
