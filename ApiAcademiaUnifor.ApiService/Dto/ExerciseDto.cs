namespace ApiAcademiaUnifor.ApiService.Dto
{
    public class ExerciseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Reps { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public int WorkoutId { get; set; }
        public int? EquipmentId { get; set; }

    }
}
