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
        // Tratamento para salvar endereços incluindo Usuario.
        if (Plano.Usuario is not null)
        {
          var Usuario = await db.Usuarios.FindAsync(Plano.Usuario.Id);
          if (Usuario is not null)
          {
            Plano.Usuario = Usuario;
          }
        }
        else
        {
          return Results.BadRequest("Usuario com Id é obrigatório");
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
        Plano.valor = PlanoAlterado.valor;
        Plano.plano = PlanoAlterado.plano;
        Plano.Nome = PlanoAlterado.Nome;

        // Tratamento para salvar endereços incluindo Usuario.
        if (Plano.Usuario is not null)
        {
          var Usuario = await db.Usuarios.FindAsync(Plano.Usuario.Id);
          if (Usuario is not null)
          {
            Plano.Usuario = Usuario;
          }
        }
        else
        {
          return Results.BadRequest("Usuario com Id é obrigatório");
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
