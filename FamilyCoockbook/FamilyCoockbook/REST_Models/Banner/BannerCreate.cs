using FamilyCookbook.Model;
using System.ComponentModel.DataAnnotations;

namespace FamilyCookbook.REST_Models.Banner
{
    public class BannerCreate : ImageDTO
    {
        [Required, StringLength(255, ErrorMessage = "The destination url exceeds maximum allowed number of characters: 255")]
        public string Destination { get; set; }
        [Required]
        public int BannerType { get; set; }
    }
}
