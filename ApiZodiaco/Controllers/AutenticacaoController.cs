using Microsoft.AspNetCore.Mvc;
using AstrologiaAPI.Models;
using AstrologiaAPI.Logic;

namespace AstrologiaAPI.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class AutenticacaoController : ControllerBase
    {
        [HttpPost]
        public IActionResult Autenticar([FromBody] Usuario usuario)
        {
            var usuarioAutenticado = AutenticacaoLogic.Autenticar(usuario.Nickname, usuario.Senha);

            if (usuarioAutenticado == null)
                return Unauthorized("Credenciais inválidas");

            return Ok(new
            {
                Nickname = usuarioAutenticado.Nickname,
                Plano = usuarioAutenticado.Plano
            });
        }
    }


}
