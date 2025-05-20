using ApiZodiaco.Models.Response;
using AstrologiaAPI.Data;
using AstrologiaAPI.Logic;
using AstrologiaAPI.Models;
using AstrologiaAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AstrologiaAPI.Controllers
{
    [ApiController]
    [Route("api/funcionalidades")]
    public class FuncionalidadesController : ControllerBase
    {
        private readonly IConfiguration _config;

        public FuncionalidadesController(IConfiguration config)
        {
            _config = config;
        }

        [Authorize]
        [HttpGet("horoscopo/{nickname}")]
        public async Task<IActionResult> ObterHoroscopo(string nickname)
        {
            try
            {
                var plano = User.FindFirst("plano")?.Value;

                if (plano != "Avançado")
                    return StatusCode(403, new RespostaEntity
                    {
                        Sucesso = false,
                        Mensagem = "Esta funcionalidade é exclusiva para usuários do plano Avançado."
                    });

                var resultado = await HoroscopoLogic.ObterHoroscopo(nickname, _config["ApiNinjasKey"]);

                if (resultado == null)
                    return NotFound(new RespostaEntity
                    {
                        Sucesso = false,
                        Mensagem = "Não foi possível obter o horóscopo."
                    });

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter horóscopo: {ex.Message}");
                return BadRequest(new RespostaEntity
                {
                    Sucesso = false,
                    Mensagem = "Erro interno ao processar o horóscopo."
                });
            }
        }

        [HttpGet("signo/{nickname}")]
        public IActionResult ConsultarSigno(string nickname)
        {
            try
            {
                var login = UsuarioDb.Logins.FirstOrDefault(l =>
                    l.Nickname.Equals(nickname, StringComparison.OrdinalIgnoreCase));

                if (login == null)
                    return NotFound(new RespostaEntity { Sucesso = false, Mensagem = "Usuário não encontrado." });

                var usuario = UsuarioDb.Usuarios.FirstOrDefault(u => u.LoginId == login.Id);
                if (usuario == null)
                    return NotFound(new RespostaEntity { Sucesso = false, Mensagem = "Dados do usuário não encontrados." });

                var signo = SignoUtils.ObterSigno(usuario.DataNascimento);
                if (signo == null)
                    return NotFound(new RespostaEntity { Sucesso = false, Mensagem = "Não foi possível determinar o signo." });

                return Ok(new
                {
                    signo = signo.Nome,
                    elemento = signo.Elemento,
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao consultar signo: {ex.Message}");
                return BadRequest(new RespostaEntity
                {
                    Sucesso = false,
                    Mensagem = "Erro interno ao consultar signo."
                });
            }
        }

        [HttpGet("frase")]
        public async Task<IActionResult> ObterFraseInspiradora()
        {
            try
            {
                var apiKey = _config["ApiNinjasKey"];

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);

                var url = "https://api.api-ninjas.com/v1/quotes";
                var resposta = await client.GetAsync(url);

                if (!resposta.IsSuccessStatusCode)
                    return BadRequest(new RespostaEntity
                    {
                        Sucesso = false,
                        Mensagem = "Não foi possível obter a frase do dia."
                    });

                var json = await resposta.Content.ReadAsStringAsync();
                var dados = JsonSerializer.Deserialize<List<JsonElement>>(json);

                if (dados == null || dados.Count == 0)
                    return NotFound(new RespostaEntity { Sucesso = false, Mensagem = "Nenhuma frase foi retornada." });

                var quoteIngles = dados[0].GetProperty("quote").GetString();
                var autor = dados[0].GetProperty("author").GetString();

                var quoteTraduzida = await TradutorUtils.TraduzirTextoAsync(quoteIngles ?? "");

                return Ok(new
                {
                    frase = quoteTraduzida,
                    autor = autor
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter frase: {ex.Message}");
                return BadRequest(new RespostaEntity
                {
                    Sucesso = false,
                    Mensagem = "Erro interno ao consultar a frase."
                });
            }
        }

        [Authorize]
        [HttpGet("bicho/{nickname}")]
        public IActionResult ObterBichoDoDia(string nickname)
        {
            try
            {
                var plano = User.FindFirst("plano")?.Value;

                if (plano != "Avançado")
                    return StatusCode(403, new RespostaEntity
                    {
                        Sucesso = false,
                        Mensagem = "Esta funcionalidade é exclusiva para usuários do plano Avançado."
                    });

                var login = UsuarioDb.Logins.FirstOrDefault(l => l.Nickname == nickname);
                if (login == null)
                    return NotFound(new RespostaEntity { Sucesso = false, Mensagem = "Usuário não encontrado." });

                var (numero, animal) = BichoLogic.ObterBichoDoDia(nickname);

                return Ok(new
                {
                    numero,
                    animal
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter bicho do dia: {ex.Message}");
                return BadRequest(new RespostaEntity
                {
                    Sucesso = false,
                    Mensagem = "Erro interno ao gerar o bicho do dia."
                });
            }
        }

    }
}
