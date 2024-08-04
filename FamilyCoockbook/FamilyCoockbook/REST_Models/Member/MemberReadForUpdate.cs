namespace FamilyCookbook.REST_Models.Member
{
    public class MemberReadForUpdate
    {
        

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string Bio { get; set; }

        public string Username { get; set; }

        public string Password {  get; set; }   

        public int RoleId { get; set; }
    }
}
