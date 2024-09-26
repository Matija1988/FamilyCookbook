using FamilyCookbook.Common;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Repository
{
    public sealed class CommentRepository(DapperDBContext context, 
        IErrorMessages errorMessages, 
        ISuccessResponses successResponses) 
        : AbstractRepository<Comment>(context, errorMessages, successResponses), ICommentRepository
    {
        private readonly DapperDBContext _context;
        private readonly IErrorMessages _errorMessages;
        private readonly ISuccessResponses _successResponses;

        protected override StringBuilder BuildQueryReadAll()
        {
            StringBuilder query = new StringBuilder();

            return query.Append("SELECT a.*, b.FirstName, b.LastName FROM Comment a JOIN " +
                "Member b on b.Id = a.MemberId ORDER BY a.DateCreated DESC;");
        }

        protected override StringBuilder BuildQueryReadSingle(int id)
        {
            StringBuilder query = new StringBuilder();

            return query.Append("SELECT a.*, b.FirstName, b.LastName FROM Comment a JOIN " +
                "Member b on b.Id = a.MemberId WHERE a.Id = @id");
        }
    }
}
