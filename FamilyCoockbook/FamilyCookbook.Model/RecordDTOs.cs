namespace FamilyCookbook.Model
{
    public record RecipeTagArray(
        [Required]
        int recipeId,
        [Required]
        int[] tagId);

    public record RecipeTag(int recipeId, int tagId);

    public record UserRegistry(
        [Required, StringLength(30, ErrorMessage = "Maximum allowed number of characters: 30")]
        string FirstName,
        [Required, StringLength(30, ErrorMessage = "Maximum allowed number of characters: 30")]
        string LastName,
        [Required]
        DateTime DateOfBirth,
        [Required, StringLength(50, ErrorMessage = "Maximum allowed number of characters: 50")]
        string Username,
        [Required, StringLength(255, ErrorMessage = "Maximum allowed number of characters: 255")]
        string Password);

}
