using AstrologiaAPI.Models;
using AstrologiaAPI.Data;

namespace AstrologiaAPI.Logic
{
    public class AutenticacaoLogic
    {
        public static Usuario Autenticar(string nickname, string senha)
        {
            var usuarios = UsuarioDb.Usuarios;

            return usuarios.FirstOrDefault(u =>
                u.Nickname == nickname && u.Senha == senha
            );
        }
    }
}
