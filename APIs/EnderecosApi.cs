using Microsoft.EntityFrameworkCore;

public static class EnderecosApi
{
    public static void MapProdutosApi(this WebApplication app)
    {
        var group = app.MapGroup("/enderecos");

        group.MapGet("/", async (BancoDeDados db) =>
            //select * from enderecos
            await db.Enderecos.ToListAsync()
        );

        group.MapPost("/", async (Endereco endereco, BancoDeDados db) =>
        {
            db.Enderecos.Add(endereco);
            //insert into ...
            await db.SaveChangesAsync();

            return Results.Created($"/enderecos/{endereco.Id}", endereco);
        });

        group.MapPut("/", async (int id, Endereco produtoAlterada, BancoDeDados db) =>
        {
            //select * from endereco where id = ?
            var endereco = await db.Enderecos.FindAsync(id);
            if (endereco is null)
            {
                return Results.NotFound();
            }

            endereco.Rua = produtoAlterada.Rua;
            endereco.Numero = produtoAlterada.Numero;
            endereco.Bairro = produtoAlterada.Bairro;
            endereco.Cidade = produtoAlterada.Cidade;
            endereco.CEP = produtoAlterada.CEP;

            //update...
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (int id, BancoDeDados db) =>
        {
            if (await db.Enderecos.FindAsync(id) is Endereco endereco)
            {
                db.Enderecos.Remove(endereco);
                //delete from ...
                await db.SaveChangesAsync();
                return Results.NoContent();
            }
            return Results.NotFound();

        });
    }
}