using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FamilyCookbook.REST_Models.Member
{
    public class MemberCreate
    {

        [StringLength(30, ErrorMessage ="Maximum allowed number of characters: 30")]
        
        public string FirstName { get; set; }

        [StringLength(30, ErrorMessage = "Maximum allowed number of characters: 30")]

        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [AllowNull]
        public string Bio { get; set; }

        [StringLength(50, ErrorMessage = "Maximum allowed number of characters: 50")]

        public string Username { get; set; }

        [StringLength(255, ErrorMessage = "Maximum allowed number of characters: 255")]

        public string Password { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}
