using FamilyCookbook.Common.Filters;
using FamilyCookbook.Model;
using FamilyCookbook.Respository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Repository.Common
{
    public interface IPictureRespository : IRepository<Picture, PictureFilter>
    {
        Task<MessageResponse> CreateAsync(Picture entity);

    }
}
