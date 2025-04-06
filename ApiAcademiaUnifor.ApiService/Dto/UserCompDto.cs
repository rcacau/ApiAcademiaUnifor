namespace ApiAcademiaUnifor.ApiService.Dto
{
    public class UserCompDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string BirthDate { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public bool IsAdmin { get; set; } = false;
        public List<WorkoutDto> Workouts { get; set; } = new();
    }
}
