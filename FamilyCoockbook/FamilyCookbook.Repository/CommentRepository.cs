using FamilyCookbook.Common;
using FamilyCookbook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Repository
{
    public class CommentRepository(DapperDBContext context, 
        IErrorMessages errorMessages, 
        ISuccessResponses successResponses) 
        : AbstractRepository<Comment>(context, errorMessages, successResponses)
    {
        private readonly DapperDBContext _context;
        private readonly IErrorMessages _errorMessages;
        private readonly ISuccessResponses _successResponses;
    }
}
