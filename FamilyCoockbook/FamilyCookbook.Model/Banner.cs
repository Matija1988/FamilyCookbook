namespace FamilyCookbook.Model
{
    public class Banner : Image
    {
        [Key]
        public int Id { get; set; }

        public string Destination { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public bool IsActive { get; set; }

        public int BannerType { get; set; }


    }
}
