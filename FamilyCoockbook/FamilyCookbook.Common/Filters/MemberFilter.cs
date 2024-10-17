using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Common.Filters
{
    public class MemberFilter
    {

        [StringLength(20, ErrorMessage = "Maximum allowed number of characters: 20")]
        public string? SearchByFirstName { get; set; }


        [StringLength(20, ErrorMessage = "Maximum allowed number of characters: 20")]
        public string? SearchByLastName { get; set; }

        [Range(1, 4, ErrorMessage = "1 to 4 allowed")]
        public int? SearchByRoleId { get; set; }

        [Range(0, 1, ErrorMessage = "Allowed inputs 0/false, 1/true")]
        public int? SearchByActivityStatus { get; set; }

        public DateTime? SearchByDateOfBirth { get; set; }

        [StringLength(50, ErrorMessage = "Maximum allowed number of characters: 50")]
        public string? SearchByBio { get; set; }
    }
}
