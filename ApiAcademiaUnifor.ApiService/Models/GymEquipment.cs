using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Text.Json.Serialization;

namespace ApiAcademiaUnifor.ApiService.Models
{
    [Table("gym_equipments")]
    public class GymEquipment : BaseModel
    {
        [PrimaryKey("id")]
        [Column("id")]
        public int Id { get; set; }

        [Column("category_id")]
        public int CategoryId { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("brand")]
        public string? Brand { get; set; }

        [Column("model")]
        public string? Model { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("image")]
        public string? Image { get; set; }

        [Column("operational")]
        public bool? Operational { get; set; } = true;


    }
}
