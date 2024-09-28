using System.ComponentModel.DataAnnotations;

namespace FamilyCookbook.REST_Models.Comment
{
    public class CommentModels
    {
        public sealed record CommentRead(int id,
                                          string memberFirstName,
                                          string memberLastName,
                                          int recipeId,
                                          string text,
                                          int rating,
                                          DateTime dateCreated);

        public sealed record CommentCreate(int MemberId,
            int RecipeId,
            [Range(0,5)]
            int? Rating, 
            string Text);
    }
}
