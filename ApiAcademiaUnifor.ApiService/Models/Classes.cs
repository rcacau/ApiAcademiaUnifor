using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ApiAcademiaUnifor.ApiService.Models
{
    [Table("classes")]
    public class Classes : BaseModel
    {
        [PrimaryKey("id")]
        [Column("id")]
        public int Id { get; set; }

        [Column("class_duration")]
        public string ClassDuration { get; set; } = string.Empty;

        [Column("class_time")]
        public string ClassTime { get; set; } = string.Empty;

        [Column("class_capacity")]
        public int ClassCapacity { get; set; }

        [Column("class_name")]
        public string ClassName { get; set; } = string.Empty;

        [Column("class_date")]
        public string ClassDate { get; set; } = string.Empty;

        [Column("class_type")]
        public string ClassType { get; set; } = string.Empty;

        [Column("teacher_id")]
        public int TeacherId { get; set; }

        [Column("class_list_users_id")]
        public List<int> ClassListUsers { get; set; } = new List<int>();

        [Column("class_completed")]
        public bool ClassCompleted { get; set; }

    }
}
