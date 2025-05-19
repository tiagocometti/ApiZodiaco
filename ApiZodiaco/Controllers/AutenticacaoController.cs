using Microsoft.AspNetCore.Mvc;
using AstrologiaAPI.Logic;
using AstrologiaAPI.Utils;
using ApiZodiaco.Models.Request;
using AstrologiaAPI.Models;

namespace AstrologiaAPI.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class AutenticacaoController : ControllerBase
    {
        [HttpPost]
        public IActionResult Autenticar([FromBody] LoginRequest credenciais)
        {
            var resultado = AutenticacaoLogic.Autenticar(credenciais.Nickname, credenciais.Senha);

            if (!resultado.Sucesso)
                return BadRequest(new { erro = resultado.Mensagem });

            var token = JwtUtils.GerarToken(resultado.Login.Nickname, resultado.Usuario.Plano);

            return Ok(new
            {
                token,
                nickname = resultado.Login.Nickname,
                nome = resultado.Usuario.Nome,
                plano = resultado.Usuario.Plano,
                nascimento = resultado.Usuario.DataNascimento
            });
        }

        [HttpPost("cadastro")]
        public IActionResult Cadastrar([FromBody] CadastroRequest request)
        {
            var resultado = CadastroLogic.Cadastrar(request);

            if (!resultado.Sucesso)
                return BadRequest(new { erro = resultado.Mensagem });

            var token = JwtUtils.GerarToken(resultado.Login.Nickname, resultado.Usuario.Plano);

            return Ok(new
            {
                token,
                nickname = resultado.Login.Nickname,
                nome = resultado.Usuario.Nome,
                plano = resultado.Usuario.Plano,
                nascimento = resultado.Usuario.DataNascimento,
            });

        }


    }
}