﻿namespace ApiAcademiaUnifor.ApiService.Dto
{
    public class GymEquipmentCategoryDto
    {
        public int Id { get; set; }
        public string category_name { get; set; } = string.Empty;
        public int Total { get; set; }
        public List<GymEquipmentDto> Equipments { get; set; } = new();
    }
}
