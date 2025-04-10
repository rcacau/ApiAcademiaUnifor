using System.Text.Json.Serialization;

namespace ApiAcademiaUnifor.ApiService.Dto
{
    public class GymEquipmentDto
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Brand { get; set; }

        public string? Model { get; set; }

        public int Quantity { get; set; }

        public string? Image { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Operational { get; set; } = null;



    }
}
