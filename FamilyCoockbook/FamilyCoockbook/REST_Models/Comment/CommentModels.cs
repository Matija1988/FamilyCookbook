namespace FamilyCookbook.REST_Models.Comment
{
    public class CommentModels
    {
        public sealed record CommentRead(int id,
                                          string firstName,
                                          string lastName,
                                          int recipeId,
                                          string text,
                                          int rating,
                                          DateTime dateCreated);
    }
}
