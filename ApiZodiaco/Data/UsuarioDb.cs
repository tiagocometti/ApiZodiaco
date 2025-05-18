using AstrologiaAPI.Models;
using AstrologiaAPI.Utils;

namespace AstrologiaAPI.Data
{
    public static class UsuarioDb
    {
        public static List<LoginEntity> Logins = new List<LoginEntity>
        {
            new LoginEntity { Nickname = "tiagocometti", Senha = SenhaUtils.GerarHash("senha123") },
            new LoginEntity { Nickname = "aries", Senha = SenhaUtils.GerarHash("456") }
        };

        public static List<UsuarioEntity> Usuarios = new List<UsuarioEntity>
        {
            new UsuarioEntity {
                LoginId = Logins[0].Id,
                Nome = "Tiago Cometti Lombardi",
                Plano = "Avançado",
                DataNascimento = new DateTime(1990, 7, 22)
            },
            new UsuarioEntity {
                LoginId = Logins[1].Id,
                Nome = "Ariana",
                Plano = "Básico",
                DataNascimento = new DateTime(1995, 3, 30)
            }
        };
    }
}
