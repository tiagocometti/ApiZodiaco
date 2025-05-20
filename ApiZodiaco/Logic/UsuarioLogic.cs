using ApiZodiaco.Models.Request;
using ApiZodiaco.Models.Response;
using AstrologiaAPI.Data;
using AstrologiaAPI.Models;
using AstrologiaAPI.Utils;

namespace AstrologiaAPI.Logic
{
    public class UsuarioLogic
    {
        public static RespostaEntity Cadastrar(CadastroRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Nickname) ||
                    string.IsNullOrWhiteSpace(request.Senha) ||
                    string.IsNullOrWhiteSpace(request.Nome) ||
                    string.IsNullOrWhiteSpace(request.Email) ||
                    string.IsNullOrWhiteSpace(request.Plano))
                {
                    return new RespostaEntity
                    {
                        Sucesso = false,
                        Mensagem = "Todos os campos são obrigatórios."
                    };
                }

                var signo = SignoUtils.ObterSigno(request.DataNascimento);
                if (signo == null)
                {
                    return new RespostaEntity
                    {
                        Sucesso = false,
                        Mensagem = "Não foi possível identificar o signo."
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
                    Email = request.Email,
                    Plano = request.Plano,
                    DataNascimento = request.DataNascimento,
                    Signo = signo.Id
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
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao cadastrar usuário: {ex.Message}");
                return new RespostaEntity
                {
                    Sucesso = false,
                    Mensagem = "Erro interno ao cadastrar usuário."
                };
            }
        }

        public static RespostaEntity Atualizar(string nickname, UsuarioUpdateRequest request)
        {
            try
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

                var usuario = UsuarioDb.Usuarios.FirstOrDefault(u => u.LoginId == login.Id);
                if (usuario == null)
                {
                    return new RespostaEntity
                    {
                        Sucesso = false,
                        Mensagem = "Dados do usuário não encontrados."
                    };
                }

                if (!string.IsNullOrWhiteSpace(request.Nome)) usuario.Nome = request.Nome;
                if (!string.IsNullOrWhiteSpace(request.Email)) usuario.Email = request.Email;
                if (!string.IsNullOrWhiteSpace(request.Plano)) usuario.Plano = request.Plano;

                if (!string.IsNullOrWhiteSpace(request.Senha))
                {
                    login.Senha = SenhaUtils.GerarHash(request.Senha);
                }

                if (request.DataNascimento.HasValue)
                {
                    usuario.DataNascimento = request.DataNascimento.Value;

                    var signo = SignoUtils.ObterSigno(usuario.DataNascimento);
                    if (signo != null)
                        usuario.Signo = signo.Id;
                }

                UsuarioDb.Salvar();

                return new RespostaEntity
                {
                    Sucesso = true,
                    Mensagem = "Dados do usuário atualizados com sucesso.",
                    Login = login,
                    Usuario = usuario
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar usuário: {ex.Message}");
                return new RespostaEntity
                {
                    Sucesso = false,
                    Mensagem = "Erro interno ao atualizar usuário."
                };
            }
        }

        public static RespostaEntity Excluir(string nickname)
        {
            try
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

                var usuario = UsuarioDb.Usuarios.FirstOrDefault(u => u.LoginId == login.Id);
                if (usuario != null)
                {
                    UsuarioDb.Usuarios.Remove(usuario);
                }

                UsuarioDb.Logins.Remove(login);
                UsuarioDb.Salvar();

                return new RespostaEntity
                {
                    Sucesso = true,
                    Mensagem = "Conta excluída com sucesso.",
                    Login = login,
                    Usuario = usuario
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir usuário: {ex.Message}");
                return new RespostaEntity
                {
                    Sucesso = false,
                    Mensagem = "Erro interno ao excluir conta."
                };
            }
        }

    }
}
