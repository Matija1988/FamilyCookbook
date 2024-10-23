using FamilyCookbook.Common.Filters;
using FamilyCookbook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service.Common
{
    public interface IPictureService : IService<Picture, PictureFilter>
    {
        Task<MessageResponse> CreateAsync(Picture entity);

        Task<MessageResponse> UpdateAsync(int id, Picture entity);
    }
}
