using System.Net.Http.Headers;
using System.Text.Json;

namespace AstrologiaAPI.Utils
{
    public static class TradutorUtils
    {
        private const string ChaveApi = "3wl8jJlL0NGLs1XYORDQHzu0QMN9ninsh8B5itnRcofqQC5ueQFpJQQJ99BEACZoyfiXJ3w3AAAbACOGsXTo";
        private const string Regiao = "brazilsouth";

        public static async Task<string> TraduzirTextoAsync(string textoIngles)
        {
            if (string.IsNullOrWhiteSpace(textoIngles))
                return "";

            try
            {
                using var http = new HttpClient();

                http.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ChaveApi);
                http.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Region", Regiao);
                http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var corpo = new[] { new { Text = textoIngles } };

                var resposta = await http.PostAsJsonAsync(
                    "https://api.cognitive.microsofttranslator.com/translate?api-version=3.0&from=en&to=pt",
                    corpo
                );

                if (!resposta.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Falha na tradução: código {resposta.StatusCode}");
                    return textoIngles;
                }

                var json = await resposta.Content.ReadFromJsonAsync<JsonElement>();
                var textoTraduzido = json[0].GetProperty("translations")[0].GetProperty("text").GetString();

                return textoTraduzido ?? textoIngles;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao traduzir texto: {ex.Message}");
                return textoIngles;
            }
        }
    }
}
