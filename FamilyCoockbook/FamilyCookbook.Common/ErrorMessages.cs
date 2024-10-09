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
            return new StringBuilder("Entity with id: " + id + " not found!");
        }

        public StringBuilder NotFound(int id, Exception ex)
        {
            return new StringBuilder("Entity with id: " + id + " not found! " + ex);
        }

        public  StringBuilder ErrorAccessingDb(string entityName) 
        {
            return new StringBuilder("Error accessing database!!! Unable to get " + entityName + "! ");
        }

        public StringBuilder ErrorAccessingDb(string entityName, Exception e)
        {
            return new StringBuilder("Error accessing database!!! Unable to get " + entityName + "! " + e);
        }

        public StringBuilder ErrorCreatingEntity(string entityName)
        {
            return new StringBuilder("Error inserting entity into database!!! " +
                "Unable to create " + entityName + "! ");
        }

        public StringBuilder ErrorCreatingEntity(string entityName, Exception ex)
        {
            return new StringBuilder("Error inserting entity into database!!! Unable to create " + entityName + "! " +
                "EXCEPTION DATA: " + ex);
        }

        public StringBuilder NotFoundByGuid()
        {
            return new StringBuilder("Member not found!!!");
        }

        public StringBuilder DeleteConstriantError(string tiedEntity)
        {
            return new StringBuilder($"The entity is tied to {tiedEntity} in the database. " +
                $"If you wish to permanently delete this entry contact your application administrator!");
        }


        public StringBuilder NestedEntityWithIdFound(string entityName, string nestedEntity)
        {
            return new StringBuilder($"Unable create entity {entityName}. " +
                $"No {nestedEntity} with requested ids found.");
        }

        public StringBuilder InvalidUsername()
        {
            return new StringBuilder("Invalid username!!!");
        }


    }
}
