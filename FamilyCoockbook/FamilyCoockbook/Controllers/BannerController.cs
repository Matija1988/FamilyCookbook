﻿using FamilyCookbook.Common.Enums;
using FamilyCookbook.Mapping.MapperWrappers;
using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Banner;
using FamilyCookbook.Service.Common;
using FamilyCookbook.Strategy;
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
        private readonly IImageProcessor _imageProcessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BannerController(IWebHostEnvironment webHostEnvironment,
            IBannerService service, IMapper<Banner, BannerRead, BannerCreate> mapper,
            IImageProcessor imageProcessor) 
            : base(service,mapper)
        {
            _service = service;
            _mapper = mapper;
            _imageProcessor = imageProcessor;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateAsync(BannerCreate newBanner)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool chkBlob = string.IsNullOrEmpty(newBanner.ImageBlob);

            var existingBanner = await _service.GetAllAsync().ContinueWith(b => 
            b.Result.Items.Find(x => x.Name == newBanner.ImageName));

            var banner = (Banner?)await _imageProcessor
                .DelegateStrategy(newBanner, existingBanner, _webHostEnvironment.WebRootPath, ImageEnum.SmallBox);

            banner.Destination = newBanner.Destination;
            banner.BannerType = (int)ImageEnum.SmallBox;
            
            var response = await _service.CreateAsync(banner);

            if (!response.IsSuccess) 
            { 
                return BadRequest(response.Message.ToString()); 
            }
            
            return Ok(response.Message.ToString());
        }
    }
}
