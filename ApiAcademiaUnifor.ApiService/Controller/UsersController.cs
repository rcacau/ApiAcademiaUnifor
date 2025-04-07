using Microsoft.AspNetCore.Mvc;
using ApiAcademiaUnifor.ApiService.Service;
using ApiAcademiaUnifor.ApiService.Dto;

namespace ApiAcademiaUnifor.ApiService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(UserService _usersService) : ControllerBase
    {
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateDto authenticateDto)
        {
            try
            {
                var retorno = await _usersService.authenticate(authenticateDto);

                if (retorno)
                    return Ok(new { message = "Autenticado com sucesso!" });

                return Unauthorized(new { message = "Falha na autenticação!" });

            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


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
            catch (Exception ex)
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

        [HttpGet("complete")]
        public async Task<IActionResult> getWE()
        {
            try
            {
                var retorno = await _usersService.GetWorkoutExercise();
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
