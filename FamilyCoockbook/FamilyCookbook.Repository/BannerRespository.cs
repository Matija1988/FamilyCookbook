using Dapper;
using FamilyCookbook.Common;
using FamilyCookbook.Common.Filters;
using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Repository
{
    public class BannerRespository : AbstractRepository<Banner, BannerFilter>, IBannerRepository
    {
        private readonly DapperDBContext _dbContext;
        private readonly IErrorMessages _errorMessages;
        private readonly ISuccessResponses _successResponses;
        public BannerRespository(DapperDBContext context, 
            IErrorMessages errorMessages, 
            ISuccessResponses successResponses) : base(context, errorMessages, successResponses)
        {
            _dbContext = context;
            _errorMessages = errorMessages;
            _successResponses = successResponses;
        }

    }
}
