
using Microsoft.EntityFrameworkCore;

public class BancoDeDados : DbContext
{

    //Configuração do banco de dados MySql
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseMySQL("server=localhost;port=3306;database=Escola;" +
         "user=root;password=vvfg0023");
    }


    //Tabelas do banco de dados
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    //...

}