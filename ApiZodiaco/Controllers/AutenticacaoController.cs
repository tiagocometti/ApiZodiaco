using Microsoft.AspNetCore.Mvc;
using AstrologiaAPI.Logic;
using AstrologiaAPI.Utils;
using ApiZodiaco.Models.Request;
using AstrologiaAPI.Models;
using ApiZodiaco.Models.Response;

namespace AstrologiaAPI.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class AutenticacaoController : ControllerBase
    {
        [HttpPost]
        public IActionResult Autenticar([FromBody] LoginRequest credenciais)
        {
            try
            {
                var resultado = AutenticacaoLogic.Autenticar(credenciais.Nickname, credenciais.Senha);

                if (!resultado.Sucesso)
                    return BadRequest(resultado);

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
            catch (Exception ex)
            {
                Console.WriteLine($"Erro interno na autenticação: {ex.Message}");
                return BadRequest(new RespostaEntity
                {
                    Sucesso = false,
                    Mensagem = "Erro interno ao autenticar."
                });
            }
        }
    }
}
