using Microsoft.EntityFrameworkCore;

public static class UsuariosApi
{
  public static void MapusuariosApi(this WebApplication app)
  {
    var group = app.MapGroup("/usuarios");

    group.MapGet("/", async (BancoDeDados db) =>
      {
        //select * from usuarios
        return await db.Usuarios.Include(c => c.Planos).ToListAsync();
      }
    );

    group.MapPost("/", async (Usuario usuario, BancoDeDados db) =>
      {
        Console.WriteLine($"usuario: {usuario}");

        // Tratamento para salvar endereços com e sem Ids.
        usuario.Planos = await TratarPlanos(usuario, db);
        
        db.Usuarios.Add(usuario);
        //insert into...
        await db.SaveChangesAsync();

        return Results.Created($"/usuarios/{usuario.Id}", usuario);
      }
    );

    group.MapPut("/{id}", async (int id, Usuario usuarioAlterado, BancoDeDados db) =>
      {
        //select * from usuarios where id = ?
        var usuario = await db.Usuarios.FindAsync(id);
        if (usuario is null)
        {
            return Results.NotFound();
        }
        usuario.Nome = usuarioAlterado.Nome;
        usuario.Telefone = usuarioAlterado.Telefone;
        usuario.Email = usuarioAlterado.Email;
        usuario.CPF = usuarioAlterado.CPF;

        // Tratamento para salvar endereços com e sem Ids.
        usuario.Planos = await TratarPlanos(usuario, db);

        //update....
        await db.SaveChangesAsync();

        return Results.NoContent();
      }
    );


    async Task<List<Plano>> TratarPlanos(usuario usuario, BancoDeDados db)
    {
      List<Plano> Planos = new();
      if (usuario is not null && usuario.Planos is not null 
          && usuario.Planos.Count > 0){

        foreach (var Plano in usuario.Planos)
        {
          Console.WriteLine($"Endereço: {Plano}");
          if (Plano.Id > 0)
          {
            var eExistente = await db.Planos.FindAsync(Plano.Id);
            if (eExistente is not null)
            {
              Planos.Add(eExistente);
            }
          }
          else
          {
            Planos.Add(Plano);
          }
        }
      }
      return Planos;
    }

    group.MapDelete("/{id}", async (int id, BancoDeDados db) =>
      {
        if (await db.Usuarios.FindAsync(id) is Usuario usuario)
        {
          //Operações de exclusão
          db.Usuarios.Remove(usuario);
          //delete from...
          await db.SaveChangesAsync();
          return Results.NoContent();
        }
        return Results.NotFound();
      }
    );
  }
}
