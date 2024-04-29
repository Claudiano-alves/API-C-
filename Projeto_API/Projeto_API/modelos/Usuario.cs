public class Usuario{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public int Idade { get; set;}
    public int CPF { get; set;}
    public string? Email { get; set;}
    public int Telefone { get; set;}

    public List<Plano>? Planos { get; set;}
}