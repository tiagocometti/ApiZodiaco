using AstrologiaAPI.Data;
using AstrologiaAPI.Models;
using AstrologiaAPI.Utils;
using System.Text.Json;

namespace AstrologiaAPI.Logic
{
    public class HoroscopoLogic
    {
        public static async Task<HoroscopoResponse?> ObterHoroscopo(string nickname, string apiKey)
        {
            var login = UsuarioDb.Logins.FirstOrDefault(l =>
                l.Nickname.Equals(nickname, StringComparison.OrdinalIgnoreCase));

            if (login == null) return null;

            var usuario = UsuarioDb.Usuarios.FirstOrDefault(u => u.LoginId == login.Id);
            if (usuario == null) return null;

            var signo = CalcularSigno(usuario.DataNascimento);
            if (signo == null) return null;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);

            var url = $"https://api.api-ninjas.com/v1/horoscope?zodiac={signo}";
            var resposta = await client.GetAsync(url);

            if (!resposta.IsSuccessStatusCode) return null;

            var json = await resposta.Content.ReadFromJsonAsync<JsonElement>();
            var texto = json.GetProperty("horoscope").GetString();
            var data = json.GetProperty("date").GetString();

            string textoTraduzido = await TradutorUtils.TraduzirTextoAsync(texto);

            return new HoroscopoResponse
            {
                Signo = signo,
                Data = data ?? "",
                Texto = textoTraduzido ?? ""
            };
        }

        private static string? CalcularSigno(DateTime nascimento)
        {
            var d = nascimento.Day;
            var m = nascimento.Month;

            return (m, d) switch
            {
                (1, <= 19) => "capricorn",
                (1, _) => "aquarius",
                (2, <= 18) => "aquarius",
                (2, _) => "pisces",
                (3, <= 20) => "pisces",
                (3, _) => "aries",
                (4, <= 19) => "aries",
                (4, _) => "taurus",
                (5, <= 20) => "taurus",
                (5, _) => "gemini",
                (6, <= 20) => "gemini",
                (6, _) => "cancer",
                (7, <= 22) => "cancer",
                (7, _) => "leo",
                (8, <= 22) => "leo",
                (8, _) => "virgo",
                (9, <= 22) => "virgo",
                (9, _) => "libra",
                (10, <= 22) => "libra",
                (10, _) => "scorpio",
                (11, <= 21) => "scorpio",
                (11, _) => "sagittarius",
                (12, <= 21) => "sagittarius",
                (12, _) => "capricorn",
                _ => null
            };
        }
    }
}
