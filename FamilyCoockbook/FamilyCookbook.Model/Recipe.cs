namespace FamilyCookbook.Model
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Text { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [ForeignKey("PictureId")]
        public Picture Picture { get; set; }

        public double AverageRating { get; set; }

        public IList<Member> Members { get; set; } = new List<Member>();

        public IList<Tag>? Tags { get; set; } = new List<Tag>();
    }
}
