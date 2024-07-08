using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Common
{
    public static class ErrorMessages
    {
        public static StringBuilder NotFound(int id)
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("Entity with id: " + id + " not found!");
        }

        public static StringBuilder ErrorAccessingDb(string entityName) 
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("Error accessing database!!! Unable to get " + entityName);
        }

        public static StringBuilder ErrorCreatingEntity(string entityName)
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("Error inserting entity into database!!! Unable to create " + entityName);
        }


    }
}
