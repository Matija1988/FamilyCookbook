using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Model
{
    public class Banner
    {
        [Key]
        public int Id { get; set; }

        public string Location { get; set; }

        public string Destination { get; set; }

        public string DestinationName { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdate { get; set; }

        public bool IsActive { get; set; }

        public string BannerType { get; set; }
    }
}
