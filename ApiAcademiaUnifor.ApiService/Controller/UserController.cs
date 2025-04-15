using Microsoft.AspNetCore.Mvc;
using ApiAcademiaUnifor.ApiService.Service;
using ApiAcademiaUnifor.ApiService.Dto;

namespace ApiAcademiaUnifor.ApiService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(UserService _usersService) : ControllerBase
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
        public async Task<IActionResult> Post([FromBody] UserDto userDto)
        {
            try
            {
                var retorno = await _usersService.Post(userDto);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserDto userDto)
        {
            try
            {
                var retorno = await _usersService.Put(userDto, id);
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
        public async Task<IActionResult> GetAllCompleteUsers()
        {
            try
            {
                var retorno = await _usersService.GetAllCompleteUsers();
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        

        [HttpGet("complete/{id}")]
        public async Task<IActionResult> GetCompleteUserByUserId(int id)
        {
            try
            {
                var retorno = await _usersService.GetCompleteUserByUserId(id);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
