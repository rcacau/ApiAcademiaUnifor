using Microsoft.AspNetCore.Mvc;
using ApiAcademiaUnifor.ApiService.Service;
using ApiAcademiaUnifor.ApiService.Dto;

namespace ApiAcademiaUnifor.ApiService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(UsersService _usersService) : ControllerBase
    {
        

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var retorno = await _usersService.GetAll();
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserInsertDto userInsertDto)
        {
            try
            {
                var retorno = await _usersService.Post(userInsertDto);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserInsertDto userInsertDto)
        {
            try
            {
                var retorno = await _usersService.Put(userInsertDto, id);
                return Ok(retorno);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var retorno = await _usersService.Delete(id);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
