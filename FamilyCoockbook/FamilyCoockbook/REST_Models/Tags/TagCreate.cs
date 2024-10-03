using System.ComponentModel.DataAnnotations;

namespace FamilyCookbook.REST_Models.Tags
{
    public class TagCreate
    {
        [StringLength(20, ErrorMessage ="Max lenght: 20 characters!")]
        public string Text { get; set; }
    }
}
