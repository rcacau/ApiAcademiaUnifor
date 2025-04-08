using ApiAcademiaUnifor.ApiService.Service;
using Microsoft.AspNetCore.Mvc;

namespace ApiAcademiaUnifor.ApiService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class GymEquipmentsController(GymEquipmentService _gymEquipmentService) : ControllerBase
    {
        [HttpGet("equipment")]
        public async Task<IActionResult> GetAllEquipments()
        {
            try
            {
                var retorno = await _gymEquipmentService.GetAllEquipments();
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("category")]
        public async Task<IActionResult> GetAllCategorys()
        {
            try
            {
                var retorno = await _gymEquipmentService.GetAllCategorys();
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("category/{id}")]
        public async Task<IActionResult> GetCategoryCompleteById(int id)
        {
            try
            {
                var retorno = await _gymEquipmentService.GetCategoryCompleteById(id);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
