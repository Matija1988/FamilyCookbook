using FamilyCookbook.Common;
using FamilyCookbook.Common.Filters;
using FamilyCookbook.Mapping;
using FamilyCookbook.Mapping.MapperWrappers;
using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Member;
using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;

namespace FamilyCookbook.Controllers
{
    [ApiController]
    [Route("api/v0/[controller]")]
    [EnableCors("CorsPolicy")]
    public class MemberController : AbstractController<Member, MemberRead, MemberCreate, MemberFilter>
    {
        private readonly  IMemberService _service;
        private readonly IMapper<Member, MemberRead, MemberCreate> _mapper;

        public MemberController(IMemberService service, 
            IMapper<Member, MemberRead, MemberCreate> mapper) 
            : base(service, mapper)
        {
            _service = service;
            _mapper = mapper;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("{uniqueId:guid}")]

        public async Task<IActionResult> GetByGuid(Guid uniqueId)
        {
            var response = await _service.GetByGuidAsync(uniqueId);

            if (response.Success == false)
            {
                return NotFound(response.Message.ToString());
            }

            var mapper = new MemberMapper();

            var member = mapper.MemberGetById(response.Items);

            return Ok(member);

        }
        [Authorize(Roles = "Admin, Moderator, Contributor")]
        [HttpGet]
        [Route("members")]
        public async Task<IActionResult> PaginateAsync([FromQuery] Paging paging, [FromQuery] MemberFilter filter )
        {
            var response = await _service.PaginateAsync(paging, filter);

            if (response.Success == false) 
            {
                return NotFound(response.Message.ToString());
            }

            var mapper = new MemberMapper();

            var members = mapper.MemberToMemberReadList(response.Items.Value);

            var finalResponse = new PaginatedList<List<MemberRead>>();

            finalResponse.Items = members;
            finalResponse.TotalCount = response.TotalCount;
            finalResponse.PageCount = response.PageCount;

            return Ok(finalResponse);
        }

        [Authorize(Roles = "Admin, Moderator, Contributor")]
        [HttpGet]
        [Route("search/{condition}")]
        public async Task<IActionResult> SearchMembersAsync(string condition)
        {
            var response = await _service.SearchMemberByCondition(condition);

            var mapper = new MemberMapper();

            var members = mapper.MemberToMemberReadList(response.Items);
            
            RepositoryResponse<List<MemberRead>> finalResponse = new RepositoryResponse<List<MemberRead>>();

            finalResponse.Items = members;


            if (response.Success == false)
            {   
                return NotFound(response.Message.ToString());
            }
            return Ok(finalResponse);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("create")] 
        public async Task<IActionResult> CreateAsync(MemberCreate memberCreate)
        {
            var mapper = new MemberMapper();

                var member = mapper.MemberCreateToMember(memberCreate);

                var response = await _service.CreateAsync(member);

                if(response.IsSuccess == false)
                {
                    return BadRequest(response.Message.ToString());
                }   

            return Ok(response.Message.ToString());    
        }

        [Authorize(Roles = "Admin, Moderator, Contributor, Member")]
        [HttpGet]
        [Route("findByUsername/{username}")]

        public async Task<IActionResult> FindByUsernameAsync(string username)
        {
            var response = await _service.GeByUsernameAsync(username);
                
            if( response.Success == false)
            {
                return BadRequest(response.Message.ToString());
            }

            var member = _mapper.MapReadToDto(response.Items.Value);
            
            return Ok(member);
        }

       
    }
}
