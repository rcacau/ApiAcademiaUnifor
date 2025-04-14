using ApiAcademiaUnifor.ApiService.Dto;
using ApiAcademiaUnifor.ApiService.Service;
using Microsoft.AspNetCore.Mvc;

namespace ApiAcademiaUnifor.ApiService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class GymEquipmentCategoryController(GymEquipmentCategoryService _gymEquipmentCategoryService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var retorno = await _gymEquipmentCategoryService.GetAll();
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var retorno = await _gymEquipmentCategoryService.GetById(id);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(GymEquipmentCategoryDto gymEquipmentCategoryDto)
        {
            try
            {
                var retorno = await _gymEquipmentCategoryService.Post(gymEquipmentCategoryDto);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(GymEquipmentCategoryDto gymEquipmentCategoryDto, int id)
        {
            try
            {
                var retorno = await _gymEquipmentCategoryService.Put(gymEquipmentCategoryDto, id);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var retorno = await _gymEquipmentCategoryService.Delete(id);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
