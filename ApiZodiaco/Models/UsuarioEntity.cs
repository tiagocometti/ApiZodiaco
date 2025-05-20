public class UsuarioEntity
{
    public Guid LoginId { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Plano { get; set; }
    public string Signo { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }

}
