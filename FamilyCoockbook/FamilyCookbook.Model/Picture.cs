namespace FamilyCookbook.Model
{
    public class Picture : Image
    {
        [Key]
        public int Id { get; set; }

        public bool IsActive { get; set; }
    }
}
