namespace ApiAcademiaUnifor.ApiService.Dto
{
    public class WorkoutDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public List<ExerciseDto> Exercises { get; set; } = new();
    }
}
