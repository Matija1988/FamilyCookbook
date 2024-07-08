using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Common
{
    public  static class SuccessResponses
    {
        public static StringBuilder EntityDeleted(string entityName)
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("Entity: " + entityName + " deleted!");
        }
    }
}
