using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Common.Exceptions
{
    public sealed class InvalidServerException : Exception
    {
        public InvalidServerException(string message) : base(message)
        {
            
        }
    }
}
