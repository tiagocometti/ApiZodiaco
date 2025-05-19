using Microsoft.AspNetCore.Mvc;
using System;

namespace AstrologiaAPI.Controllers
{
    [ApiController]
    [Route("api/pagamento")]
    public class PagamentoController : ControllerBase
    {
        [HttpGet("gerar")]
        public IActionResult GerarPixSimples()
        {

            var codigoPix = GerarCodigoPixFake();

            return Ok(new
            {
                codigoPix
            });
        }

        private string GerarCodigoPixFake()
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
            return $"0002012636BR.GOV.BCB.PIX0114+5581999999995204000053039865406009SAO PAULO62070503***6304{guid}";
        }
    }
}
