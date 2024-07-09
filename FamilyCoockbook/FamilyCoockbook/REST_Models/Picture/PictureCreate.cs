using System.ComponentModel.DataAnnotations;

namespace FamilyCookbook.REST_Models.Picture
{
    public class PictureCreate
    {
        [StringLength(100, ErrorMessage ="Maximum allowed number of characters :50")]
        public string Name { get; set; }

        [StringLength(255, ErrorMessage ="Maximum allowed number of characters: 255")]
        public string Location { get; set; }
    }
}
