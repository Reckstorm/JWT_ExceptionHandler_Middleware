namespace _04._11_ASP.Models
{
    public class UserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IFormFile? AvatarImg { get; set; }
    }
}
