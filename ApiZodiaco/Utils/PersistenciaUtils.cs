using System.Text.Json;
using AstrologiaAPI.Models;
using System.Text;

namespace AstrologiaAPI.Utils
{
    public static class PersistenciaUtils
    {
        private const string LoginPath = "logins.json";
        private const string UsuarioPath = "usuarios.json";

        public static void SalvarLogins(List<LoginEntity> logins)
        {
            var json = JsonSerializer.Serialize(logins, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(LoginPath, json, Encoding.UTF8);
        }

        public static void SalvarUsuarios(List<UsuarioEntity> usuarios)
        {
            var json = JsonSerializer.Serialize(usuarios, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(UsuarioPath, json, Encoding.UTF8);
        }

        public static List<LoginEntity> CarregarLogins()
        {
            if (!File.Exists(LoginPath)) return new List<LoginEntity>();
            var json = File.ReadAllText(LoginPath);
            return JsonSerializer.Deserialize<List<LoginEntity>>(json) ?? new List<LoginEntity>();
        }

        public static List<UsuarioEntity> CarregarUsuarios()
        {
            if (!File.Exists(UsuarioPath)) return new List<UsuarioEntity>();
            var json = File.ReadAllText(UsuarioPath);
            return JsonSerializer.Deserialize<List<UsuarioEntity>>(json) ?? new List<UsuarioEntity>();
        }
    }
}
