using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ApiAcademiaUnifor.ApiService.Models
{
    [Table("workout_exercises")]
    public class WorkoutExercise : BaseModel
    {
        [PrimaryKey("id")]
        [Column("id")]
        public int Id { get; set; }

        [Column("workout_id")]
        public int WorkoutId { get; set; }

        [Column("exercise_id")]
        public int ExerciseId { get; set; }
    }
}
