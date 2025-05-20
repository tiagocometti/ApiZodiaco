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
            try
            {
                var login = UsuarioDb.Logins.FirstOrDefault(l =>
                    l.Nickname.Equals(nickname, StringComparison.OrdinalIgnoreCase));

                if (login == null) return null;

                var usuario = UsuarioDb.Usuarios.FirstOrDefault(u => u.LoginId == login.Id);
                if (usuario == null) return null;

                var signo = usuario.Signo;
                if (string.IsNullOrWhiteSpace(signo)) return null;

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
                    Signo = usuario.Signo,
                    Data = data ?? "",
                    Texto = textoTraduzido ?? ""
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter horóscopo: {ex.Message}");
                return null;
            }
        }
    }
}
