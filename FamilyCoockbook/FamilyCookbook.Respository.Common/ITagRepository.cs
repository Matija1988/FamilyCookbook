using FamilyCookbook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Repository.Common
{
    public interface ITagRepository
    {
        Task<RepositoryResponse<List<Tag>>> GetAllAsync();   
    }
}
