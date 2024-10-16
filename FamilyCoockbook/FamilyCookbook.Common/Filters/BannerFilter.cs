using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Common.Filters
{
    public class BannerFilter
    {
        public string? SearchByName { get; set; }

        public string? SearchByDestination { get; set; }

        public int? SearchByActivityStatus { get; set; }
    }
}
