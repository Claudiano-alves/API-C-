using Microsoft.EntityFrameworkCore;

public static class PlanosApi
{
  public static void MapPlanosApi(this WebApplication app)
  {
    var group = app.MapGroup("/Planos");

    group.MapGet("/", async (BancoDeDados db) =>
      //select * from Planos
      await db.Planos.ToListAsync()
    );

    group.MapPost("/", async (Plano Plano, BancoDeDados db) =>
      {
        // Tratamento para salvar endereços incluindo cliente.
        if (Plano.Cliente is not null)
        {
          var cliente = await db.Clientes.FindAsync(Plano.Cliente.Id);
          if (cliente is not null)
          {
            Plano.Cliente = cliente;
          }
        }
        else
        {
          return Results.BadRequest("Cliente com Id é obrigatório");
        }

        db.Planos.Add(Plano);
        //insert into...
        await db.SaveChangesAsync();

        return Results.Created($"/Planos/{Plano.Id}", Plano);
      }
    );

    group.MapPut("/{id}", async (int id, Plano PlanoAlterado, BancoDeDados db) =>
      {
        //select * from Planos where id = ?
        var Plano = await db.Planos.FindAsync(id);
        if (Plano is null)
        {
            return Results.NotFound();
        }
        Plano.Rua = PlanoAlterado.Rua;
        Plano.Numero = PlanoAlterado.Numero;
        Plano.Bairro = PlanoAlterado.Bairro;
        Plano.Cidade = PlanoAlterado.Cidade;
        Plano.CEP = PlanoAlterado.CEP;

        // Tratamento para salvar endereços incluindo cliente.
        if (Plano.Cliente is not null)
        {
          var cliente = await db.Clientes.FindAsync(Plano.Cliente.Id);
          if (cliente is not null)
          {
            Plano.Cliente = cliente;
          }
        }
        else
        {
          return Results.BadRequest("Cliente com Id é obrigatório");
        }

        //update....
        await db.SaveChangesAsync();

        return Results.NoContent();
      }
    );

    group.MapDelete("/{id}", async (int id, BancoDeDados db) =>
      {
        if (await db.Planos.FindAsync(id) is Plano Plano)
        {
          //Operações de exclusão
          db.Planos.Remove(Plano);
          //delete from...
          await db.SaveChangesAsync();
          return Results.NoContent();
        }
        return Results.NotFound();
      }
    );
  }
}
