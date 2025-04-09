namespace ApiAcademiaUnifor.ApiService.Dto
{
    public class WorkoutDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public List<ExerciseDto> Exercises { get; set; } = new();
    }
}
