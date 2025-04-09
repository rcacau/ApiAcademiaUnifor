namespace ApiAcademiaUnifor.ApiService.Dto
{
    public class GymEquipmentInsertDto
    {
        public int CategoryId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Brand { get; set; }

        public string? Model { get; set; }

        public int Quantity { get; set; }

        public string? Image { get; set; }
    }
}
