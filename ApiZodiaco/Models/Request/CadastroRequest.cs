public class CadastroRequest
{
    public string Nickname { get; set; }
    public string Senha { get; set; }
    public string Nome { get; set; }
    public string Plano { get; set; } // <- agora obrigatório
    public DateTime DataNascimento { get; set; }
    public string Email { get; set; }
}
