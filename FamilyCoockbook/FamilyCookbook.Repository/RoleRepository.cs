namespace FamilyCookbook.Repository
{
    public sealed class RoleRepository : AbstractRepository<Role, RoleFilter>, IRoleRepository
    {
        public RoleRepository
            (DapperDBContext context, IErrorMessages errorMessages, ISuccessResponses successResponses) 
            : base(context, errorMessages, successResponses)
        {

        }
    }
}
