using ApiAcademiaUnifor.ApiService.Dto;
using ApiAcademiaUnifor.ApiService.Models;
using ApiAcademiaUnifor.ApiService.Service.Base;
using Supabase.Gotrue;
using System.Net.Http.Headers;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using Supabase.Gotrue.Mfa;

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
                    Image = e.Image,
                    Operational = e.Operational == false ? false : null

                }).ToList();

                return dtoList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao carregar os equipamentos: {ex.Message}");
            }
        }


        public async Task<GymEquipmentDto> GetEquipmentById(int id)
        {
            try
            {
                var result = await _supabase
                .From<GymEquipment>()
                .Where(e => e.Id == id)
                .Get();

                var equipmentId = result.Models.FirstOrDefault();

                if (equipmentId == null)
                    throw new Exception("Equipamento não encontrado");

                var dtoList = new GymEquipmentDto
                {
                    Id = equipmentId.Id,
                    CategoryId = equipmentId.CategoryId,
                    Name = equipmentId.Name,
                    Brand = equipmentId.Brand,
                    Model = equipmentId.Model,
                    Quantity = equipmentId.Quantity,
                    Image = equipmentId.Image,
                    Operational = equipmentId.Operational == false ? false : null

                };

                return dtoList;
            }
            catch (Exception ex)
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
                throw new Exception($"Erro ao carregar só as categorias: {ex.Message}");
            }

        }

        public async Task<List<GymEquipmentCategoryDto>> GetAllCategoriesWithEquipments()
        {
            try
            {
                var categorias = await _supabase.From<Models.GymEquipmentCategory>().Get();
                var equipamentos = await _supabase.From<GymEquipment>().Get();

                var categoriasComEquipamentos = categorias.Models.Select(categoria =>
                {
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
                            Operational = e.Operational == false ? false : null

                        })
                        .ToList();

                    return new GymEquipmentCategoryDto
                    {
                        Id = categoria.Id,
                        category_name = categoria.category_name,
                        Total = categoria.Total,
                        Equipments = equipamentosDto
                    };
                }).ToList();

                return categoriasComEquipamentos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao carregar categorias com equipamentos: {ex.Message}");
            }
        }


        public async Task<GymEquipmentCategoryDto> GetCategoryCompleteById(int id)
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
                        Operational = e.Operational == false ? false : null
                    })
                    .ToList();

                
                var categoriaCompleta = new GymEquipmentCategoryDto
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
                throw new Exception($"Erro ao carregar categoria pelo id: {ex.Message}");
            }
        }

        public async Task<GymEquipmentDto> PostEquipment(GymEquipmentDto gymEquipmentDto)
        {
            try
            {
                var categoriesResponse = await _supabase.From<Models.GymEquipmentCategory>().Get();
                var category = categoriesResponse.Models.FirstOrDefault(c => c.Id == gymEquipmentDto.CategoryId);

                if (category == null)
                    throw new Exception("Categoria informada não existe.");

                var lista = await _supabase.From<Models.GymEquipment>().Get();

                int id = lista.Models.Any() ? lista.Models.Max(e => e.Id) : 0;

                var gymEquipment = new GymEquipment
                {
                    Id = id + 1,
                    CategoryId = gymEquipmentDto.CategoryId,
                    Name = gymEquipmentDto.Name,
                    Brand = gymEquipmentDto.Brand,
                    Model = gymEquipmentDto.Model,
                    Quantity = gymEquipmentDto.Quantity,
                    Image = gymEquipmentDto.Image,
                    Operational = gymEquipmentDto.Operational == false ? false : null


                };

                var equipmentResponse = await _supabase.From<Models.GymEquipment>().Insert(gymEquipment);

                category.Total += gymEquipment.Quantity;
                await _supabase.From<Models.GymEquipmentCategory>().Update(category);

                var result = equipmentResponse.Models.FirstOrDefault();

                if (result == null)
                    throw new Exception("Não foi possivel inserir o equipamento");

                return new GymEquipmentDto
                {
                    Id = result.Id,
                    CategoryId = result.CategoryId,
                    Name = result.Name,
                    Brand = result.Brand,
                    Model = result.Model,
                    Quantity = result.Quantity,
                    Image = result.Image,
                    Operational = result.Operational == false ? false : null

                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao inserir equipamento: {ex.Message}");
            }
        }


        public async Task<GymEquipmentCategoryDto> PostCategory(GymEquipmentCategoryInsertDto gymEquipmentCategoryInsertDto)
        {
            try
            {
                var lista = await _supabase.From<Models.GymEquipmentCategory>().Get();

                int id = lista.Models.Any() ? lista.Models.Max(e => e.Id) : 0;

                var gymEquipmentCategory = new GymEquipmentCategory
                {
                    Id = id + 1,
                    category_name = gymEquipmentCategoryInsertDto.category_name,
                    Total = gymEquipmentCategoryInsertDto.Total
                };

                var categoryResponse = await _supabase.From<Models.GymEquipmentCategory>().Insert(gymEquipmentCategory);

                var result = categoryResponse.Models.FirstOrDefault();

                if (result == null)
                    throw new Exception("Erro ao inserir a categoria");

                var gymCategory = new GymEquipmentCategoryDto
                {
                    Id = result.Id,
                    category_name = result.category_name,
                    Total = result.Total,
                };

                return gymCategory;

            }
            catch(Exception ex)
            {
                throw new Exception($"Erro ao inserir categoria: {ex.Message}");
            }
        }

        public async Task<GymEquipmentDto> PutEquipment(GymEquipmentDto gymEquipmentDto, int id)
        {
            try
            {
                var equipmentResponse = await _supabase.From<Models.GymEquipment>().Where(c => c.Id==id).Single();

                if (equipmentResponse == null)
                    throw new Exception("Equipamento informada não existe.");

                var categoriesResponse = await _supabase.From<Models.GymEquipmentCategory>().Get();
                var category_old = categoriesResponse.Models.FirstOrDefault(c => c.Id == equipmentResponse.CategoryId);

                if (category_old == null)
                    throw new Exception("Categoria informada não existe.");


                category_old.Total -= equipmentResponse.Quantity;

                await _supabase.From<Models.GymEquipmentCategory>().Update(category_old);

                equipmentResponse.CategoryId = gymEquipmentDto.CategoryId;
                equipmentResponse.Name = gymEquipmentDto.Name;
                equipmentResponse.Brand = gymEquipmentDto.Brand;
                equipmentResponse.Model = gymEquipmentDto.Model;
                equipmentResponse.Quantity = gymEquipmentDto.Quantity;
                equipmentResponse.Image = gymEquipmentDto.Image;
                equipmentResponse.Operational = gymEquipmentDto.Operational == false ? false : null;




                var category_new = categoriesResponse.Models.FirstOrDefault(c => c.Id == equipmentResponse.CategoryId);

                if (category_new == null)
                    throw new Exception("Categoria informada não existe.");

                category_new.Total += equipmentResponse.Quantity;

                var equipment = await equipmentResponse.Update<GymEquipment>();

                var result = equipment.Models.FirstOrDefault();

                if (result == null)
                    throw new Exception("Não foi possivel inserir o equipamento");

                return new GymEquipmentDto
                {
                    Id = result.Id,
                    CategoryId = result.CategoryId,
                    Name = result.Name,
                    Brand = result.Brand,
                    Model = result.Model,
                    Quantity = result.Quantity,
                    Image = result.Image,
                    Operational = result.Operational == false ? false : null

                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao inserir equipamento: {ex.Message}");
            }
        }

        public async Task<GymEquipmentCategoryDto> PutCategory(GymEquipmentCategoryInsertDto gymEquipmentCategoryInsertDto, int id)
        {
            try
            {
                var categoryResponse = await _supabase.From<Models.GymEquipmentCategory>().Where(c => c.Id == id).Single();

                if (categoryResponse == null)
                    throw new Exception("Categoria não encontrada");


                categoryResponse.category_name = gymEquipmentCategoryInsertDto.category_name;
                categoryResponse.Total = gymEquipmentCategoryInsertDto.Total;


                var category = await categoryResponse.Update<GymEquipmentCategory>();

                var result = category.Models.FirstOrDefault();

                if (result == null)
                    throw new Exception("Erro ao inserir a categoria");

                var gymCategory = new GymEquipmentCategoryDto
                {
                    Id = result.Id,
                    category_name = result.category_name,
                    Total = result.Total,
                };

                return gymCategory;

            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao inserir categoria: {ex.Message}");
            }
        }

        public async Task<GymEquipmentDto> DeleteEquipment(int id)
        {
            try
            {
                var equipmentResponse = await _supabase.From<GymEquipment>().Where(x => x.Id == id).Single();

                if (equipmentResponse == null)
                    throw new Exception("Usuário não encontrado.");


                var categoriesResponse = await _supabase.From<Models.GymEquipmentCategory>().Get();
                var category = categoriesResponse.Models.FirstOrDefault(c => c.Id == equipmentResponse.CategoryId);

                if (category == null)
                    throw new Exception("Categoria informada não existe.");

                category.Total -= equipmentResponse.Quantity;

                GymEquipmentDto response = await GetEquipmentById(id);

                await _supabase.From<GymEquipment>().Where(x => x.Id == id).Delete();
                

                return response;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<GymEquipmentCategoryDto> DeleteCategory(int id)
        {
            try
            {
                var categoryResponse = await _supabase.From<GymEquipmentCategory>().Where(x => x.Id == id).Single();

                if (categoryResponse == null)
                    throw new Exception("Usuário não encontrado.");

                GymEquipmentCategoryDto response = await GetCategoryCompleteById(id);
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
