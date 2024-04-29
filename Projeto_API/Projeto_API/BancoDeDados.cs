using Microsoft.EntityFrameworkCore;

public class BancoDeDados : DbContext
{

    //Configuração da conexão
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseMySQL("server=localhost;port=3306;database=exemplo;user=root;password=");
     
    }

    //Mapeamento das tabelas
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Plano> Planos { get; set; }
}
