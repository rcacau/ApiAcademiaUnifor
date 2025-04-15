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

        public async Task<List<GymEquipmentDto>> GetAll()
        {
            try
            {
                var equipmentRecords = await _supabase
                    .From<GymEquipment>()
                    .Get();

                var equipmentDtoList = equipmentRecords.Models.Select(equipment => new GymEquipmentDto
                {
                    Id = equipment.Id,
                    CategoryId = equipment.CategoryId,
                    Name = equipment.Name,
                    Brand = equipment.Brand,
                    Model = equipment.Model,
                    Quantity = equipment.Quantity,
                    Image = equipment.Image,
                    Operational = equipment.Operational == false ? false : null
                }).ToList();

                return equipmentDtoList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao carregar os equipamentos: {ex.Message}");
            }
        }


        public async Task<GymEquipmentDto> GetById(int id)
        {
            try
            {
                var queryResult = await _supabase
                    .From<GymEquipment>()
                    .Where(equipment => equipment.Id == id)
                    .Get();

                var equipment = queryResult.Models.FirstOrDefault();

                if (equipment == null)
                    throw new Exception("Equipamento não encontrado");

                var equipmentDto = new GymEquipmentDto
                {
                    Id = equipment.Id,
                    CategoryId = equipment.CategoryId,
                    Name = equipment.Name,
                    Brand = equipment.Brand,
                    Model = equipment.Model,
                    Quantity = equipment.Quantity,
                    Image = equipment.Image,
                    Operational = equipment.Operational == false ? false : null
                };

                return equipmentDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao carregar o equipamento: {ex.Message}");
            }
        }


        public async Task<List<GymEquipmentDto>> GetByCategoryId(int categoryId)
        {
            try
            {
                var queryResult = await _supabase
                    .From<GymEquipment>()
                    .Where(equipment => equipment.CategoryId == categoryId)
                    .Get();

                var equipmentList = queryResult.Models.ToList();

                if (equipmentList == null)
                    throw new Exception("Nenhum equipamento encontrado para esta categoria.");

                var equipmentDtoList = equipmentList.Select(equipment => new GymEquipmentDto
                {
                    Id = equipment.Id,
                    CategoryId = equipment.CategoryId,
                    Name = equipment.Name,
                    Brand = equipment.Brand,
                    Model = equipment.Model,
                    Quantity = equipment.Quantity,
                    Image = equipment.Image,
                    Operational = equipment.Operational == false ? false : null
                }).ToList();

                return equipmentDtoList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao carregar equipamentos por categoria: {ex.Message}");
            }
        }


        public async Task<GymEquipmentDto> Post(GymEquipmentDto newEquipmentDto)
        {
            try
            {
                var categoryQuery = await _supabase
                    .From<Models.GymEquipmentCategory>()
                    .Get();

                var category = categoryQuery.Models.FirstOrDefault(c => c.Id == newEquipmentDto.CategoryId);

                if (category == null)
                    throw new Exception("Categoria informada não existe.");

                var existingEquipments = await _supabase
                    .From<Models.GymEquipment>()
                    .Get();

                int nextId = existingEquipments.Models.Any()
                    ? existingEquipments.Models.Max(e => e.Id) + 1
                    : 1;

                var newEquipment = new GymEquipment
                {
                    Id = nextId,
                    CategoryId = newEquipmentDto.CategoryId,
                    Name = newEquipmentDto.Name,
                    Brand = newEquipmentDto.Brand,
                    Model = newEquipmentDto.Model,
                    Quantity = newEquipmentDto.Quantity,
                    Image = newEquipmentDto.Image,
                    Operational = newEquipmentDto.Operational == false ? false : null
                };

                var insertResult = await _supabase
                    .From<Models.GymEquipment>()
                    .Insert(newEquipment);

                category.Total += newEquipment.Quantity;

                await _supabase
                    .From<Models.GymEquipmentCategory>()
                    .Update(category);

                var insertedEquipment = insertResult.Models.FirstOrDefault();

                if (insertedEquipment == null)
                    throw new Exception("Não foi possível inserir o equipamento.");

                return new GymEquipmentDto
                {
                    Id = insertedEquipment.Id,
                    CategoryId = insertedEquipment.CategoryId,
                    Name = insertedEquipment.Name,
                    Brand = insertedEquipment.Brand,
                    Model = insertedEquipment.Model,
                    Quantity = insertedEquipment.Quantity,
                    Image = insertedEquipment.Image,
                    Operational = insertedEquipment.Operational == false ? false : null
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao inserir equipamento: {ex.Message}");
            }
        }



        public async Task<GymEquipmentDto> Put(GymEquipmentDto updatedEquipmentDto, int equipmentId)
        {
            try
            {
                var equipmentQuery = await _supabase
                    .From<Models.GymEquipment>()
                    .Where(e => e.Id == equipmentId)
                    .Single();

                if (equipmentQuery == null)
                    throw new Exception("Equipamento informado não existe.");

                var categoryQuery = await _supabase
                    .From<Models.GymEquipmentCategory>()
                    .Get();

                var previousCategory = categoryQuery.Models
                    .FirstOrDefault(c => c.Id == equipmentQuery.CategoryId);

                if (previousCategory == null)
                    throw new Exception("Categoria anterior não existe.");

                previousCategory.Total -= equipmentQuery.Quantity;
                await _supabase.From<Models.GymEquipmentCategory>().Update(previousCategory);

                equipmentQuery.CategoryId = updatedEquipmentDto.CategoryId;
                equipmentQuery.Name = updatedEquipmentDto.Name;
                equipmentQuery.Brand = updatedEquipmentDto.Brand;
                equipmentQuery.Model = updatedEquipmentDto.Model;
                equipmentQuery.Quantity = updatedEquipmentDto.Quantity;
                equipmentQuery.Image = updatedEquipmentDto.Image;
                equipmentQuery.Operational = updatedEquipmentDto.Operational == false ? false : null;

                var newCategory = categoryQuery.Models
                    .FirstOrDefault(c => c.Id == equipmentQuery.CategoryId);

                if (newCategory == null)
                    throw new Exception("Nova categoria informada não existe.");

                newCategory.Total += equipmentQuery.Quantity;
                await _supabase.From<Models.GymEquipmentCategory>().Update(newCategory);

                var updateResult = await equipmentQuery.Update<GymEquipment>();
                var updatedEquipment = updateResult.Models.FirstOrDefault();

                if (updatedEquipment == null)
                    throw new Exception("Não foi possível atualizar o equipamento.");

                return new GymEquipmentDto
                {
                    Id = updatedEquipment.Id,
                    CategoryId = updatedEquipment.CategoryId,
                    Name = updatedEquipment.Name,
                    Brand = updatedEquipment.Brand,
                    Model = updatedEquipment.Model,
                    Quantity = updatedEquipment.Quantity,
                    Image = updatedEquipment.Image,
                    Operational = updatedEquipment.Operational == false ? false : null
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar equipamento: {ex.Message}");
            }
        }




        public async Task<GymEquipmentDto> Delete(int equipmentId)
        {
            try
            {
                var equipment = await _supabase
                    .From<GymEquipment>()
                    .Where(e => e.Id == equipmentId)
                    .Single();

                if (equipment == null)
                    throw new Exception("Equipamento não encontrado.");

                var categoryQuery = await _supabase
                    .From<Models.GymEquipmentCategory>()
                    .Get();

                var relatedCategory = categoryQuery.Models
                    .FirstOrDefault(c => c.Id == equipment.CategoryId);

                if (relatedCategory == null)
                    throw new Exception("Categoria relacionada não existe.");

                relatedCategory.Total -= equipment.Quantity;
                await _supabase.From<Models.GymEquipmentCategory>().Update(relatedCategory);

                GymEquipmentDto deletedEquipmentDto = await GetById(equipmentId);

                await _supabase.From<GymEquipment>()
                    .Where(e => e.Id == equipmentId)
                    .Delete();

                return deletedEquipmentDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao excluir equipamento: {ex.Message}");
            }
        }



    }


}
