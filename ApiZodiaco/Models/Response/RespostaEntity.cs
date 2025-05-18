using AstrologiaAPI.Models;

namespace ApiZodiaco.Models.Response
{
    public class RespostaEntity
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public LoginEntity Login { get; set; }
        public UsuarioEntity Usuario { get; set; }
    }
}
