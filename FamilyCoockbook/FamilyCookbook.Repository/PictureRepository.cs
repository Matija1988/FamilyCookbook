using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;

namespace FamilyCookbook.Repository
{
    public sealed class PictureRepository : AbstractRepository<Picture>, IPictureRespository
    {
        
        public PictureRepository(DapperDBContext context, IErrorMessages errorMessages, ISuccessResponses successResponses) 
            : base(context, errorMessages, successResponses) 
        {
         
        }
    }
}
