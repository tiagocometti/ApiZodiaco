using ApiZodiaco.Models.Response;
using AstrologiaAPI.Data;
using AstrologiaAPI.Models;
using AstrologiaAPI.Utils;

namespace AstrologiaAPI.Logic
{
    public class CadastroLogic
    {
        public static RespostaEntity Cadastrar(CadastroRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Nickname) ||
                string.IsNullOrWhiteSpace(request.Senha) ||
                string.IsNullOrWhiteSpace(request.Nome) ||
                string.IsNullOrWhiteSpace(request.Email))
            {
                return new RespostaEntity
                {
                    Sucesso = false,
                    Mensagem = "Nickname, senha, nome e e-mail são obrigatórios."
                };
            }


            if (UsuarioDb.Logins.Any(l => l.Nickname == request.Nickname))
            {
                return new RespostaEntity
                {
                    Sucesso = false,
                    Mensagem = "Nickname já está em uso."
                };
            }

            var novoLogin = new LoginEntity
            {
                Nickname = request.Nickname,
                Senha = SenhaUtils.GerarHash(request.Senha)
            };

            var novoUsuario = new UsuarioEntity
            {
                LoginId = novoLogin.Id,
                Nome = request.Nome,
                Plano = request.Plano ?? "",
                DataNascimento = request.DataNascimento
            };

            UsuarioDb.Logins.Add(novoLogin);
            UsuarioDb.Usuarios.Add(novoUsuario);
            UsuarioDb.Salvar();

            return new RespostaEntity
            {
                Sucesso = true,
                Mensagem = "Usuário cadastrado com sucesso.",
                Login = novoLogin,
                Usuario = novoUsuario
            };
        }
    }
}
