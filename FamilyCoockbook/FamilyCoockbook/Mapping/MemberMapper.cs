﻿using FamilyCookbook.Model;
using FamilyCookbook.REST_Models.Member;
using Riok.Mapperly.Abstractions;

namespace FamilyCookbook.Mapping
{
    [Mapper]
    public  partial class MemberMapper
    {
        public  partial List<MemberRead> MemberToMemberReadList(List<Member> member);

        public  partial MemberRead MemberToMemberRead(Member member);

        public partial Member MemberCreateToMember(MemberCreate memberCreate);
    }
}
