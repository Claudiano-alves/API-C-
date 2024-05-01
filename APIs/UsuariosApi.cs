using Microsoft.EntityFrameworkCore;

public static class UsuariosApi
{
    public static void MapPessoasApi(this WebApplication app)
    {
        var group = app.MapGroup("/usuarios");

        group.MapGet("/", async (BancoDeDados db) =>
            //select * from usuarios
            await db.Usuarios.ToListAsync()
        );

        group.MapPost("/", async (Usuario usuario, BancoDeDados db) =>
        {
            db.Usuarios.Add(usuario);
            //insert into ...
            await db.SaveChangesAsync();

            return Results.Created($"/usuarios/{usuario.Id}", usuario);
        });

        group.MapPut("/", async (int id, Usuario pessoaAlterada, BancoDeDados db) =>
        {
            //select * from usuario where id = ?
            var usuario = await db.Usuarios.FindAsync(id);
            if (usuario is null)
            {
                return Results.NotFound();
            }

            usuario.Nome = pessoaAlterada.Nome;
            usuario.CPF = pessoaAlterada.CPF;

            //update...
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (int id, BancoDeDados db) =>
        {
            if (await db.Usuarios.FindAsync(id) is Usuario usuario)
            {
                db.Usuarios.Remove(usuario);
                //delete from ...
                await db.SaveChangesAsync();
                return Results.NoContent();
            }
            return Results.NotFound();

        });
    }
}