using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Model
{
    public abstract class ImageDTO
    {
        public string ImageName { get; set; }

        public string? ImageBlob { get; set; }
    }
}
