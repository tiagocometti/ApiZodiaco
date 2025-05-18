using ApiZodiaco.Models.Response;
using AstrologiaAPI.Data;
using AstrologiaAPI.Utils;

namespace AstrologiaAPI.Logic
{
    public class AutenticacaoLogic
    {
        public static RespostaEntity Autenticar(string nickname, string senha)
        {
            var login = UsuarioDb.Logins.FirstOrDefault(l => l.Nickname == nickname);

            if (login == null)
            {
                return new RespostaEntity
                {
                    Sucesso = false,
                    Mensagem = "Usuário não encontrado."
                };
            }

            if (!SenhaUtils.VerificarSenha(senha, login.Senha))
            {
                return new RespostaEntity
                {
                    Sucesso = false,
                    Mensagem = "Senha incorreta."
                };
            }

            var usuario = UsuarioDb.Usuarios.FirstOrDefault(u => u.LoginId == login.Id);
            if (usuario == null)
            {
                return new RespostaEntity
                {
                    Sucesso = false,
                    Mensagem = "Usuário sem dados associados."
                };
            }

            return new RespostaEntity
            {
                Sucesso = true,
                Mensagem = "Autenticação bem-sucedida.",
                Login = login,
                Usuario = usuario
            };
        }
    }
}
