namespace AstrologiaAPI.Models
{
    public class LoginEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // funciona como chave primária
        public string Nickname { get; set; }
        public string Senha { get; set; }
    }
}
