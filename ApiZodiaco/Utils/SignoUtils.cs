using AstrologiaAPI.Models;
using System.Text.Json;

namespace AstrologiaAPI.Utils
{
    public static class SignoUtils
    {
        private static List<SignoEntity>? _signos;

        public static SignoEntity? ObterSigno(DateTime nascimento)
        {
            if (_signos == null)
            {
                try
                {
                    var json = File.ReadAllText("FakeDb/signos.json");
                    _signos = JsonSerializer.Deserialize<List<SignoEntity>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });


                    if (_signos == null || !_signos.Any())
                        return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao carregar signos: {ex.Message}");
                    return null;
                }
            }

            var nascimentoMMDD = nascimento.ToString("MM-dd");

            foreach (var signo in _signos!)
            {
                if (signo.Inicio.CompareTo(signo.Fim) <= 0)
                {
                    if (nascimentoMMDD.CompareTo(signo.Inicio) >= 0 && nascimentoMMDD.CompareTo(signo.Fim) <= 0)
                        return signo;
                }
                else
                {
                    if (nascimentoMMDD.CompareTo(signo.Inicio) >= 0 || nascimentoMMDD.CompareTo(signo.Fim) <= 0)
                        return signo;
                }
            }

            return null;
        }
    }
}
