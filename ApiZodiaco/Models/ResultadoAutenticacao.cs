namespace AstrologiaAPI.Models
{
    public class ResultadoAutenticacao
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public LoginEntity Login { get; set; }
        public UsuarioEntity Usuario { get; set; }
    }
}
