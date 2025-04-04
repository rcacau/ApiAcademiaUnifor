using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiAcademiaUnifor.ApiService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcademiaController : ControllerBase
    {
        [HttpGet("teste")]
        public IActionResult Teste()
        {
            var resultado = new
            {
                sucesso = true,
                mensagem = "API funcionando corretamente!",
                dados = new
                {
                    nome = "Academia UNIFOR",
                    endereco = "Av. Washington Soares, 1321 - Fortaleza/CE",
                    horarioFuncionamento = "06:00 às 22:00",
                    modalidades = new[] { "Musculação", "CrossFit", "Pilates", "Funcional" }
                }
            };

            return Ok(resultado);
        }
    }
}
