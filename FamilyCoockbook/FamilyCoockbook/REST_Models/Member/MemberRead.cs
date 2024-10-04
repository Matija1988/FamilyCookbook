using FamilyCookbook.Model;
using System.ComponentModel;

namespace FamilyCookbook.REST_Models.Member
{
    public class MemberRead
    {
        
        public int Id { get; set; }

        public Guid UniqueId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string Bio { get; set; }

        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }


    }
}
