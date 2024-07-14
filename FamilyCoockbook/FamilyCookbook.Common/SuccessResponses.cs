using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Common
{
    public  class SuccessResponses : ISuccessResponses
    {
        public SuccessResponses()
        {
            
        }
        public  StringBuilder EntityDeleted(string entityName)
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("Entity: " + entityName + " deleted!");
        }

        public  StringBuilder EntityCreated()
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("Entity created!");
        }

        public  StringBuilder EntityUpdated()
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("Entity updated!");
        }
    }
}
