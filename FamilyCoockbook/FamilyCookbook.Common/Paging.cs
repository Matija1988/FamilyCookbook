using System.ComponentModel.DataAnnotations;

namespace FamilyCookbook.Common
{
    public class Paging
    {
        [Range(1, int.MaxValue, ErrorMessage = "Please enter valid integer value: 1 or above")]
        public int PageSize { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter valid integer value: 1 or above")]
        public int PageNumber { get; set; }
    }
}
