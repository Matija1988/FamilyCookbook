using FamilyCookbook.Common;
using FamilyCookbook.Common.Filters;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using System.Text;

namespace FamilyCookbook.Repository
{
    public sealed class PictureRepository : AbstractRepository<Picture, PictureFilter>, IPictureRespository
    {
        
        public PictureRepository(DapperDBContext context, IErrorMessages errorMessages, ISuccessResponses successResponses) 
            : base(context, errorMessages, successResponses) 
        {
         
        }
        protected override StringBuilder BuildQueryReadAll()
        {
            StringBuilder query = new("SELECT* FROM Picture p WHERE p.Id IN ");
            query.Append("(SELECT MIN(p2.Id) FROM Picture p2 GROUP BY p2.Name) Order by Id DESC;");
            return query;
        }
    }
}
