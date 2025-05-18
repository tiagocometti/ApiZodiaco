namespace AstrologiaAPI.Models
{
    public class UsuarioEntity
    {
        public Guid LoginId { get; set; } 
        public string Plano { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Nome { get; set; }
    }
}
