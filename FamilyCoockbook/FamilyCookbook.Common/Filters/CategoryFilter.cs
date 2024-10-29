using System.ComponentModel.DataAnnotations;

namespace FamilyCookbook.Common.Filters
{
    public class CategoryFilter
    {
        [StringLength(50, ErrorMessage ="Maximum allowed number of characters: 50")]
        public string? SearchByName { get; set; }

        [Range(0,1, ErrorMessage ="Allowed inputs 0/false, 1/true")]
        public int? SearchByActivityStatus { get; set; }
    }
}
