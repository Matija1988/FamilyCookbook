namespace FamilyCookbook.Model
{
    public class MemberRecipe
    {
        [ForeignKey("MemberId")]
        public int MemberId { get; set; }

        [ForeignKey("RecipeId")]
        public int RecipeId { get; set; }
    }
}
