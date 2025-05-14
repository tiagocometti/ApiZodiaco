using AstrologiaAPI.Models;

namespace AstrologiaAPI.Data
{
    public static class UsuarioDb
    {
        public static List<Usuario> Usuarios = new List<Usuario>
        {
            new Usuario { Nickname = "leo", Senha = "123", Plano = "Avançado" },
            new Usuario { Nickname = "aries", Senha = "456", Plano = "Básico" }
        };
    }
}
