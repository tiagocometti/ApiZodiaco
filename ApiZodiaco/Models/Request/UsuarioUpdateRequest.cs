﻿public class UsuarioUpdateRequest
{
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Plano { get; set; }
    public DateTime? DataNascimento { get; set; }
    public string? Senha { get; set; }  // 👈 novo campo
}
