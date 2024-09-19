using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FamilyCookbook.REST_Models.Category
{
    public class CategoryCreate
    {
        [StringLength(50, ErrorMessage = "Maximum allowed number of characters: 50")]
        [Required]
        public string Name { get; set; }

        [AllowNull]
        public string Description { get; set; }
    }
}
