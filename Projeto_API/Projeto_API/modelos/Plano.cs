public class Plano
{
    public int Id { get; set; }
    public string plano { get; set; }
    public int valor { get; set; }
    public string Nome { get; set; }

    public Usuario? Usuario { get; set; }
}