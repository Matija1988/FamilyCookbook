using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Common
{
    public class MemberFilter
    {
        public string? SearchByFirstName { get; set; }

        public string? SearchByLastName { get; set; }

        public int? SearchByRoleId { get; set; }

        public int? SearchByActivityStatus { get; set; }

        public DateTime? SearchByDateOfBirth { get; set; }

        public string? SearchByBio { get; set; }
    }
}
