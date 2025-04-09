namespace ApiAcademiaUnifor.ApiService.Dto
{
    public class GymEquipmentCategoryInsertDto
    {
        public string category_name { get; set; } = string.Empty;
        public int Total { get; set; }
        public List<GymEquipmentDto> Equipments { get; set; } = new();
    }
}
