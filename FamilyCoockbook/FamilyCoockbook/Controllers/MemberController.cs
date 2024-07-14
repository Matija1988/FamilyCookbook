using FamilyCookbook.Mapping;
using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Member;
using FamilyCookbook.Service.Common;
using Microsoft.AspNetCore.Mvc;

namespace FamilyCookbook.Controllers
{
    [ApiController]
    [Route("api/v0/member")]
    public class MemberController : ControllerBase
    {
        private readonly  IMemberService _service;

        public MemberController(IMemberService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync() 
        { 
            var response = await _service.GetAllAsync();

            if (response.Success == false)
            {
                return NotFound(response.Message);
            }

            var mapper = new MemberMapper();

            var members = mapper.MemberToMemberReadList(response.Items);


            return Ok(members);
        }

        [HttpGet]
        [Route("{id:int}")]

        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _service.GetByIdAsync(id);

            if(response.Success == false)
            {
                return NotFound(response.Message);
            }

            var mapper = new MemberMapper();

            var member = mapper.MemberToMemberRead(response.Items);

            return Ok(member);  
        }

        [HttpGet]
        [Route("{uniqueId:guid}")]

        public async Task<IActionResult> GetByGuid(Guid uniqueId)
        {
            var response = await _service.GetByGuidAsync(uniqueId);

            if (response.Success == false)
            {
                return NotFound(response.Message);
            }

            var mapper = new MemberMapper();

            var member = mapper.MemberToMemberRead(response.Items);

            return Ok(member);

        }
        [HttpPost]
        [Route("create")] 
        public async Task<IActionResult> CreateAsync(MemberCreate memberCreate)
        {
            var mapper = new MemberMapper();

            var member = mapper.MemberCreateToMember(memberCreate);

            var response = await _service.CreateAsync(member);

            if(response.Success == false)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Message);    
        }

        [HttpPut]
        [Route("update/{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, MemberCreate memberCreate)
        {
            var mapper = new MemberMapper();

            var member = mapper.MemberCreateToMember(memberCreate);

            var response = await _service.UpdateAsync(id, member);

            if (response.Success == false) 
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Message);
        }

        [HttpPut]
        [Route("delete/{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await _service.DeleteAsync(id);

            if(response.Success == false)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Message);
        }

        [HttpDelete]
        [Route("permaDelete/{id:int}")]

        public async Task<IActionResult> PermaDeleteAsync(int id)
        {
            var response = await _service.PermaDeleteAsync(id);

            if(response.Success == false)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Message);
        }

        
    }
}
