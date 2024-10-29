using System.Text;

namespace FamilyCookbook.Common
{
    public interface ISuccessResponses
    {
        StringBuilder EntityDeleted(string entityName);

        StringBuilder EntityCreated();

        StringBuilder EntityUpdated();
    }
}
