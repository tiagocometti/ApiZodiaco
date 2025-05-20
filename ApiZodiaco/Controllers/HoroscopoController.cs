using Microsoft.AspNetCore.Mvc;
using AstrologiaAPI.Logic;
using AstrologiaAPI.Models;

namespace AstrologiaAPI.Controllers
{
    [ApiController]
    [Route("api/horoscopo")]
    public class HoroscopoController : ControllerBase
    {
        private readonly IConfiguration _config;

        public HoroscopoController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("{nickname}")]
        public async Task<IActionResult> ObterHoroscopo(string nickname)
        {
            var resultado = await HoroscopoLogic.ObterHoroscopo(nickname, _config["ApiNinjasKey"]);

            if (resultado == null)
                return NotFound(new { erro = "Não foi possível obter o horóscopo." });

            return Ok(resultado);
        }
    }
}
