namespace FamilyCookbook.Repository.Common
{
    public interface IRecipeRepository : IRepository<Recipe, RecipeFilter>
    {
        Task<RepositoryResponse<Recipe>> RemoveMemberFromRecipeAsync(int memberId, int recipeId);  

        Task<RepositoryResponse<Recipe>> AddMemberToRecipeAsync(MemberRecipe entity);

        Task<MessageResponse> CreateAsyncTransaction(RecipeCreateDTO entity);

        Task<MessageResponse> UpdateAsync(int id, RecipeCreateDTO entity);
        
        Task<RepositoryResponse<List<Recipe>>> GetRecipesWithoutAuthors();

        Task<RepositoryResponse<Recipe>> AddPictureToRecipeAsync(int pictureId, int recipeId);

    }
}
