using System.ComponentModel.DataAnnotations;

namespace FamilyCookbook.REST_Models.Recipe
{
    public class RecipeCreate
    {
        [StringLength(200, ErrorMessage ="Maximum allowed number of characters: 200")]
        public string Title { get; set; }


        [StringLength(200, ErrorMessage = "Maximum allowed number of characters: 200")]
        public string Subtitle { get; set; }

        public string Text { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public List<int> MemberIds { get; set; }
        
        public string PictureName { get; set; }

        public IFormFile Picture { get; set; }
 

    }
}
