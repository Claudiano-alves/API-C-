
public class Usuario
{

    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? CPF { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }

    Usuario novoUsuario = new Usuario
        {
            Id = 1, 
            Nome = "João da Silva",
            CPF = "123.456.789-00",
            Telefone = "(99) 99999-9999",
            Email = "joao@example.com"
    };

}