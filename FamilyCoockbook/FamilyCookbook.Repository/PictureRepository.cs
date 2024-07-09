using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;

namespace FamilyCookbook.Repository
{
    public class PictureRepository : AbstractRepository<Picture>, IPictureRespository
    {
        
        public PictureRepository(DapperDBContext context) : base(context) 
        {
         
        }
    }
}
