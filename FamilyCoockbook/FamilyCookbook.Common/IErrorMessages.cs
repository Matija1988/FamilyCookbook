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

        StringBuilder NotFoundByGuid();


        StringBuilder ErrorAccessingDb(string entityName);

        StringBuilder ErrorCreatingEntity(string entityName);

        StringBuilder NestedEntityWithIdFound(string entityName, string nestedEntity);

        StringBuilder InvalidUsername();
    }
}
