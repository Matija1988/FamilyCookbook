using FamilyCookbook.Mapping.MapperWrappers;
using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Banner;
using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using static FamilyCookbook.REST_Models.Banner.BannerDTO;

namespace FamilyCookbook.Controllers
{
    [ApiController]
    [Route("api/v0/[controller]")]
    public class BannerPositionController : ControllerBase
    {
        private readonly IBannerPositionService _service;
        private readonly IMapper<Banner, BannerRead, BannerCreate> _mapper;

        public BannerPositionController(IMapper<Banner, BannerRead, BannerCreate> mapper,
            IBannerPositionService service)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPut]
        [Route("setBannerPosition")]

        public async Task<IActionResult> SetBannerPosition(BannerPosition bannerPosition)
        {
            var response = await _service.AssignBannerToPosition(bannerPosition);

            if(!response.IsSuccess) 
            { 
                BadRequest(response.Message.ToString()); 
            }
            return Ok(response.Message.ToString());
        }

        [HttpGet]
        [Route("getBannerForPosition/{position:int}")]

        public async Task<IActionResult> GetBannerForPosition(int position)
        {
            var response = await _service.GetBannerForPosition(position);

            if (!response.Success)
            {
                return BadRequest(response.Message.ToString());
            }

            var banner = _mapper.MapReadToDto(response.Items);

            return Ok(banner);
        }
    }
}
