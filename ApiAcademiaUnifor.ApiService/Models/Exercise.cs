using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ApiAcademiaUnifor.ApiService.Models
{
    [Table("exercises")]
    public class Exercise : BaseModel
    {
        [PrimaryKey("id")]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("reps")]
        public string Reps { get; set; } = string.Empty;

        [Column("notes")]
        public string? Notes { get; set; }

        [Column("workout_id")]
        public int WorkoutId { get; set; }

        [Column("equipment_id")]
        public int? EquipmentId { get; set; }
    }
}
