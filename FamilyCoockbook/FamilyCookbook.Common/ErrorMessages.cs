using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Common
{
    public  class ErrorMessages : IErrorMessages
    {
        public ErrorMessages()
        {
            
        }
        public StringBuilder NotFound(int id)
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("Entity with id: " + id + " not found!");
        }

        public  StringBuilder ErrorAccessingDb(string entityName) 
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("Error accessing database!!! Unable to get " + entityName + "! ");
        }

        public  StringBuilder ErrorCreatingEntity(string entityName)
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("Error inserting entity into database!!! Unable to create " + entityName + "! ");
        }

        public StringBuilder NotFoundByGuid()
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("Member not found!!!");
        }
    }
}
