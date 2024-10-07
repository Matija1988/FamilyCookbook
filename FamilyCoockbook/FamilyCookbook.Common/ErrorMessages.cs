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

        public StringBuilder NotFound(int id, Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("Entity with id: " + id + " not found! " + ex);
        }

        public  StringBuilder ErrorAccessingDb(string entityName) 
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("Error accessing database!!! Unable to get " + entityName + "! ");
        }

        public StringBuilder ErrorAccessingDb(string entityName, Exception e)
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("Error accessing database!!! Unable to get " + entityName + "! " + e);
        }

        public StringBuilder ErrorCreatingEntity(string entityName)
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("Error inserting entity into database!!! Unable to create " + entityName + "! ");
        }

        public StringBuilder ErrorCreatingEntity(string entityName, Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("Error inserting entity into database!!! Unable to create " + entityName + "! " +
                "EXCEPTION DATA: " + ex);
        }

        public StringBuilder NotFoundByGuid()
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("Member not found!!!");
        }

        public StringBuilder DeleteConstriantError(string tiedEntity)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"The entity is tied to {tiedEntity} in the database. " +
                $"If you wish to permanently delete this entry contact your application administrator!");
            return sb;
        }


        public StringBuilder NestedEntityWithIdFound(string entityName, string nestedEntity)
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append($"Unable create entity {entityName}. No {nestedEntity} with requested ids found.");
        }

        public StringBuilder InvalidUsername()
        {
            StringBuilder sb = new StringBuilder();
            return sb.Append("Invalid username!!!");
        }


    }
}
