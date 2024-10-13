using FamilyCookbook.Mapping.MapperWrappers;
using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Banner;
using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Mvc;
using static FamilyCookbook.REST_Models.Banner.BannerDTO;

namespace FamilyCookbook.Controllers
{
    [ApiController]
    [Route("api/v0/[controller]")]
    public class BannerController : AbstractController<Banner, BannerRead, BannerCreate>
    {
        private readonly IBannerService _service;
        private readonly IMapper<Banner, BannerRead, BannerCreate> _mapper;

        public BannerController(IBannerService service, IMapper<Banner, BannerRead, BannerCreate> mapper) 
            : base(service,mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateAsync(BannerCreate banner)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool chkBlob = string.IsNullOrEmpty(banner.ImageBlob);


            throw new NotImplementedException();
        }
    }
}
