using ApiAcademiaUnifor.ApiService.Dto;
using ApiAcademiaUnifor.ApiService.Models;
using ApiAcademiaUnifor.ApiService.Service.Base;

namespace ApiAcademiaUnifor.ApiService.Service
{
    public class GymEquipmentCategoryService : ServiceBase
    {
        private readonly GymEquipmentService _gymEquipmentService;
        public GymEquipmentCategoryService(Supabase.Client supabase) : base(supabase)
        {
            _gymEquipmentService = new GymEquipmentService(supabase);
        }

        public async Task<List<GymEquipmentCategoryDto>> GetAll()
        {
            try
            {
                var categoriasResponse = await _supabase.From<GymEquipmentCategory>().Get();
                var categorias = categoriasResponse.Models.ToList();

                var categoriaDtos = new List<GymEquipmentCategoryDto>();

                foreach (var w in categorias)
                {
                    var equipamentos = await _gymEquipmentService.GetEquipmentByCategoryId(w.Id);

                    categoriaDtos.Add(new GymEquipmentCategoryDto
                    {
                        Id = w.Id,
                        category_name =w.CategoryName,
                        Total = w.Total,
                        Equipments = equipamentos
                    });
                }

                return categoriaDtos;
                
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao carregar categorias com equipamentos: {ex.Message}");
            }
        }

        public async Task<GymEquipmentCategoryDto> GetById(int id)
        {
            try
            {
                var categorias = await _supabase.From<GymEquipmentCategory>().Where(x => x.Id == id).Get();
                var categoria = categorias.Models.FirstOrDefault();
                if (categoria == null)
                    throw new Exception("Categoria não encontrada");

                var equipamentos = await _gymEquipmentService.GetEquipmentByCategoryId(id);


                var categoriaCompleta = new GymEquipmentCategoryDto
                {
                    Id = categoria.Id,
                    category_name = categoria.CategoryName,
                    Total = categoria.Total,
                    Equipments = equipamentos
                };

                return categoriaCompleta;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao carregar categoria pelo id: {ex.Message}");
            }
        }

        public async Task<GymEquipmentCategoryDto> Post(GymEquipmentCategoryDto gymEquipmentCategoryDto)
        {
            try
            {
                var lista = await _supabase.From<Models.GymEquipmentCategory>().Get();

                int nextId = lista.Models.Any() 
                    ? lista.Models.Max(e => e.Id) + 1 
                    : 1;

                var gymEquipmentCategory = new GymEquipmentCategory
                {
                    Id = nextId,
                    CategoryName = gymEquipmentCategoryDto.category_name,
                };

                await _supabase.From<Models.GymEquipmentCategory>().Insert(gymEquipmentCategory);

                gymEquipmentCategoryDto.Id = gymEquipmentCategory.Id;

                return gymEquipmentCategoryDto;

            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao inserir categoria: {ex.Message}");
            }
        }

        public async Task<GymEquipmentCategoryDto> Put(GymEquipmentCategoryDto gymEquipmentCategoryDto, int id)
        {
            try
            {
                var categoryResponse = await _supabase.From<Models.GymEquipmentCategory>().Where(c => c.Id == id).Single();

                if (categoryResponse == null)
                    throw new Exception("Categoria não encontrada");

                
                categoryResponse.CategoryName = gymEquipmentCategoryDto.category_name;


                var category = await categoryResponse.Update<GymEquipmentCategory>();

                var result = category.Models.FirstOrDefault();

                if (result == null)
                    throw new Exception("Erro ao inserir a categoria");

                var equipamentos = await _gymEquipmentService.GetEquipmentByCategoryId(id);

                var gymCategory = new GymEquipmentCategoryDto
                {
                    Id = result.Id,
                    category_name = result.CategoryName,
                    Total = result.Total,
                    Equipments = equipamentos
                };

                return gymCategory;

            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao inserir categoria: {ex.Message}");
            }
        }

        public async Task<GymEquipmentCategoryDto> Delete(int id)
        {
            try
            {
                var categoryResponse = await _supabase.From<GymEquipmentCategory>().Where(x => x.Id == id).Single();

                if (categoryResponse == null)
                    throw new Exception("Usuário não encontrado.");

                GymEquipmentCategoryDto response = await GetById(id);
                await _supabase.From<GymEquipment>().Where(x => x.CategoryId == id).Delete();
                await _supabase.From<GymEquipmentCategory>().Where(x => x.Id == id).Delete();

                return response;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
