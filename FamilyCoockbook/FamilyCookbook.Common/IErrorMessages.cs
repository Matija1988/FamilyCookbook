using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Common
{
    public interface IErrorMessages
    {
        StringBuilder NotFound(int id);

        StringBuilder NotFound(int id, Exception ex);

        StringBuilder NotFoundByGuid();


        StringBuilder ErrorAccessingDb(string entityName);


        StringBuilder ErrorAccessingDb(string entityName, Exception ex);

        StringBuilder ErrorCreatingEntity(string entityName);

        StringBuilder ErrorCreatingEntity(string entityName, Exception ex);

        StringBuilder NestedEntityWithIdFound(string entityName, string nestedEntity);

        StringBuilder InvalidUsername();
    }
}
