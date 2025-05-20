using System;

namespace AstrologiaAPI.Logic
{
    public static class BichoLogic
    {
        private static readonly string[] Animais =
        {
            "Avestruz", "Águia", "Burro", "Borboleta", "Cachorro", "Cabra", "Carneiro",
            "Camelo", "Cobra", "Coelho", "Cavalo", "Elefante", "Galo", "Gato", "Jacaré",
            "Leão", "Macaco", "Porco", "Pavão", "Peru", "Touro", "Tigre", "Urso", "Veado", "Vaca"
        };

        public static (int numero, string animal) ObterBichoDoDia(string nickname)
        {
            var hoje = DateTime.UtcNow.Date.ToString("yyyyMMdd");
            var chave = $"{nickname}-{hoje}";

            int hash = Math.Abs(chave.GetHashCode());
            int numero = (hash % 25) + 1;
            string animal = Animais[numero - 1];

            return (numero, animal);
        }
    }
}
