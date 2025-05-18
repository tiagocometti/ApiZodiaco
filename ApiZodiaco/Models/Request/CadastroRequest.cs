public class CadastroRequest
{
    public string Nickname { get; set; }
    public string Senha { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string? Plano { get; set; } // <- agora é opcional
    public DateTime DataNascimento { get; set; }
}
