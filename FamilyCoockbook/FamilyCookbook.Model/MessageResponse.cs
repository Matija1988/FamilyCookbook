using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Model
{
    public struct MessageResponse
    {
        public bool IsSuccess { get; set; }

        public StringBuilder Message { get; set; }
    }
}
