namespace PayEase_CaseStudy.DTOs
{
    public class UserWithRolesDTO
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }

}
