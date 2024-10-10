using FamilyCookbook.Model;
using System.Diagnostics.CodeAnalysis;

namespace FamilyCookbook.REST_Models.Recipe
{
    public sealed class RecipeRead
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Text { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }

        public List<MemberListRecipe> Members { get; set; }

        public string? PictureName { get; set; }

        public string PictureLocation { get; set; }

        public string PictureId { get; set; }

        public decimal AverageRating { get; set; }

        public List<Tag>? Tags { get; set; }

    }
}
