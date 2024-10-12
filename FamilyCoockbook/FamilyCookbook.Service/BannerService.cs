using FamilyCookbook.Model;
using FamilyCookbook.Repository.Common;
using FamilyCookbook.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Service
{
    public class BannerService : AbstractService<Banner>, IBannerService
    {
        private readonly IBannerRepository _repository;

        public BannerService(IBannerRepository repository) : base(repository) 
        {
            _repository = repository;
        }

    }
}
