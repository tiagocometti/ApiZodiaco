using ApiZodiaco.Models.Request;
using AstrologiaAPI.Data;
using AstrologiaAPI.Logic;
using AstrologiaAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using ApiZodiaco.Models.Response;

namespace AstrologiaAPI.Controllers
{
    [ApiController]
    [Route("api/usuario")]
    public class UsuarioController : ControllerBase
    {
        [HttpPost("cadastro")]
        public IActionResult Cadastrar([FromBody] CadastroRequest request)
        {
            try
            {
                var resultado = UsuarioLogic.Cadastrar(request);

                if (!resultado.Sucesso)
                    return BadRequest(resultado);

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
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao cadastrar: {ex.Message}");
                return BadRequest(new RespostaEntity
                {
                    Sucesso = false,
                    Mensagem = "Erro interno ao cadastrar usuário."
                });
            }
        }

        [HttpPut("atualizar/{nickname}")]
        public IActionResult Atualizar(string nickname, [FromBody] UsuarioUpdateRequest request)
        {
            try
            {
                var resultado = UsuarioLogic.Atualizar(nickname, request);

                if (!resultado.Sucesso)
                    return NotFound(resultado);

                return Ok(new
                {
                    mensagem = resultado.Mensagem,
                    nickname = resultado.Login.Nickname,
                    nome = resultado.Usuario.Nome,
                    email = resultado.Usuario.Email,
                    plano = resultado.Usuario.Plano,
                    nascimento = resultado.Usuario.DataNascimento
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar: {ex.Message}");
                return BadRequest(new RespostaEntity
                {
                    Sucesso = false,
                    Mensagem = "Erro interno ao atualizar usuário."
                });
            }
        }

        [HttpDelete("excluir/{nickname}")]
        public IActionResult Excluir(string nickname)
        {
            try
            {
                var resultado = UsuarioLogic.Excluir(nickname);

                if (!resultado.Sucesso)
                    return NotFound(resultado);

                return Ok(new { mensagem = resultado.Mensagem });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir conta: {ex.Message}");
                return BadRequest(new RespostaEntity
                {
                    Sucesso = false,
                    Mensagem = "Erro interno ao excluir conta."
                });
            }
        }

        [HttpGet("consultar/{nickname}")]
        public IActionResult Consultar(string nickname)
        {
            try
            {
                var login = UsuarioDb.Logins.FirstOrDefault(l => l.Nickname == nickname);
                if (login == null)
                    return NotFound(new RespostaEntity { Sucesso = false, Mensagem = "Usuário não encontrado." });

                var usuario = UsuarioDb.Usuarios.FirstOrDefault(u => u.LoginId == login.Id);
                if (usuario == null)
                    return NotFound(new RespostaEntity { Sucesso = false, Mensagem = "Dados do usuário não encontrados." });

                return Ok(new
                {
                    nickname = login.Nickname,
                    nome = usuario.Nome,
                    email = usuario.Email,
                    plano = usuario.Plano,
                    nascimento = usuario.DataNascimento,
                    signo = usuario.Signo
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao consultar usuário: {ex.Message}");
                return BadRequest(new RespostaEntity
                {
                    Sucesso = false,
                    Mensagem = "Erro interno ao consultar dados."
                });
            }
        }

        [HttpGet("renovar/{nickname}")]
        public IActionResult RenovarToken(string nickname)
        {
            try
            {
                var login = UsuarioDb.Logins.FirstOrDefault(l => l.Nickname == nickname);
                if (login == null)
                {
                    return NotFound(new RespostaEntity
                    {
                        Sucesso = false,
                        Mensagem = "Usuário não encontrado."
                    });
                }

                var usuario = UsuarioDb.Usuarios.FirstOrDefault(u => u.LoginId == login.Id);
                if (usuario == null)
                {
                    return NotFound(new RespostaEntity
                    {
                        Sucesso = false,
                        Mensagem = "Dados do usuário não encontrados."
                    });
                }

                var novoToken = JwtUtils.GerarToken(nickname, usuario.Plano);

                return Ok(new
                {
                    token = novoToken,
                    nickname = nickname,
                    plano = usuario.Plano,
                    mensagem = "Token renovado com sucesso."
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao renovar token: {ex.Message}");
                return BadRequest(new RespostaEntity
                {
                    Sucesso = false,
                    Mensagem = "Erro interno ao renovar token."
                });
            }
        }

    }
}
