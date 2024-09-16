using System.Diagnostics.CodeAnalysis;

namespace FamilyCookbook.REST_Models.Recipe
{
    public class RecipeRead
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Text { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public List<MemberListRecipe> Members { get; set; }

        public string? PictureName { get; set; }

        public string PictureLocation { get; set; }

        public string PictureId { get; set; }

    }
}
