using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Member;

namespace FamilyCookbook.Mapping
{
    public class MemberMapperWrapper : IMapper<Member, MemberRead, MemberCreate>
    {
        private readonly MemberMapper _mapper = new();
        public MemberRead MapReadToDto(Member entity)
        {
            return _mapper.MemberToMemberRead(entity);
        }

        public Member MapToEntity(MemberCreate dto)
        {
            return _mapper.MemberCreateToMember(dto);
        }

        public List<MemberRead> MapToReadList(List<Member> entities)
        {
            return _mapper.MemberToMemberReadList(entities);
        }
    }
}
