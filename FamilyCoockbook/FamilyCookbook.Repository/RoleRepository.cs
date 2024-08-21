using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;

namespace FamilyCookbook.Repository
{
    public sealed class RoleRepository : AbstractRepository<Role>, IRoleRepository
    {
        public RoleRepository
            (DapperDBContext context, IErrorMessages errorMessages, ISuccessResponses successResponses) 
            : base(context, errorMessages, successResponses)
        {

        }
    }
}
