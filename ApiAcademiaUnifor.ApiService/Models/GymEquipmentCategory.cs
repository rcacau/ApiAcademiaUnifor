using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ApiAcademiaUnifor.ApiService.Models
{
    [Table("gym_equipment_categories")]
    public class GymEquipmentCategory : BaseModel
    {
        [PrimaryKey("id")]
        [Column("id")]
        public int Id { get; set; }

        [Column("category_name")]
        public string category_name { get; set; } = string.Empty;

        [Column("total")]
        public int Total { get; set; }
        
    }
}
