using ApiAcademiaUnifor.ApiService.Dto;
using ApiAcademiaUnifor.ApiService.Models;
using ApiAcademiaUnifor.ApiService.Service.Base;

namespace ApiAcademiaUnifor.ApiService.Service
{
    public class GymEquipmentService : ServiceBase
    {
        public GymEquipmentService(Supabase.Client supabase) : base(supabase)
        {
        }

        public async Task<List<GymEquipmentDto>> Get()
        {
            var result = await _supabase
                .From<GymEquipment>()
                .Select("*")
                .Get();

            var dtoList = result.Models.Select(e => new GymEquipmentDto
            {
                Id = e.Id,
                CategoryId = e.CategoryId,
                Name = e.Name,
                Brand = e.Brand,
                Model = e.Model,
                Quantity = e.Quantity,
                Image = e.Image
            }).ToList();

            return dtoList;
        }

    }
}
