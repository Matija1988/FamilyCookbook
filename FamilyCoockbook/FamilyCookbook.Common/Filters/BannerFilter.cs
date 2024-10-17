using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Common.Filters
{
    public class BannerFilter
    {

        [StringLength(50, ErrorMessage = "Maximum allowed number of characters: 50")]
        public string? SearchByName { get; set; }

        [StringLength(50, ErrorMessage = "Maximum allowed number of characters: 50")]
        public string? SearchByDestination { get; set; }

        [Range(0, 1, ErrorMessage = "Allowed inputs 0/false, 1/true")]
        public int? SearchByActivityStatus { get; set; }
    }
}
