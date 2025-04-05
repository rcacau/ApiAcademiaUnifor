namespace ApiAcademiaUnifor.ApiService.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Password { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? BirthDate { get; set; }
        public string? AvatarUrl { get; set; }
        public bool IsAdmin { get; set; }
    }

}
