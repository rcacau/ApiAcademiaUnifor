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

        public async Task<List<GymEquipmentDto>> GetAllEquipments()
        {
            try
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
            catch(Exception ex)
            {
                throw new Exception($"Erro ao carregar os equipamentos: {ex.Message}");
            }
            
        }

        public async Task<List<GymEquipmentCategoryDto>> GetAllCategorys()
        {
            try
            {
                var result = await _supabase
                .From<GymEquipmentCategory>()
                .Select("*")
                .Get();

                var dtoList = result.Models.Select(e => new GymEquipmentCategoryDto
                {
                    Id = e.Id,
                    category_name = e.category_name,
                    Total = e.Total,
                }).ToList();

                return dtoList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao carregar os equipamentos: {ex.Message}");
            }

        }

        public async Task<GymEquipmentCategoryCompleteDto> GetCategoryCompleteById(int id)
        {
            try
            {
                var categorias = await _supabase.From<Models.GymEquipmentCategory>().Get();
                var equipamentos = await _supabase.From<GymEquipment>().Get();

                
                var categoria = categorias.Models.FirstOrDefault(c => c.Id == id);

                if (categoria == null)
                    throw new Exception("Categoria não encontrada");

                
                var equipamentosDto = equipamentos.Models
                    .Where(e => e.CategoryId == categoria.Id)
                    .Select(e => new GymEquipmentDto
                    {
                        Id = e.Id,
                        CategoryId = e.CategoryId,
                        Name = e.Name,
                        Brand = e.Brand,
                        Model = e.Model,
                        Quantity = e.Quantity,
                        Image = e.Image,
                    })
                    .ToList();

                
                var categoriaCompleta = new GymEquipmentCategoryCompleteDto
                {
                    Id = categoria.Id,
                    category_name = categoria.category_name,
                    Total = categoria.Total,
                    Equipments = equipamentosDto
                };

                return categoriaCompleta;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao carregar categoria: {ex.Message}");
            }
        }


    }


}
