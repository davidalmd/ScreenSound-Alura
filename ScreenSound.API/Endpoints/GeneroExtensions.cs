using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Shared.Modelos.Modelos;

namespace ScreenSound.API.Endpoints;

public static class GeneroExtensions
{
    public static void AddEndpointsGeneros(this WebApplication app)
    {
        app.MapGet("/Generos", ([FromServices] DAL<Genero> DAL) =>
        {
            return Results.Ok(EntityListToResponseList(DAL.Listar()));
        });

        app.MapGet("/Generos/{nome}", ([FromServices] DAL<Genero> DAL, string nome) =>
        {
            var genero = DAL.RecuperarPor(g => g.Nome!.ToUpper().Equals(nome.ToUpper()));
            if (genero is null) return Results.NotFound();
            var generoLimpo = new Genero()
            {
                Id = genero.Id,
                Nome = genero.Nome,
                Descricao = genero.Descricao
            };
            return Results.Ok(EntityToResponse(generoLimpo));
        });

        app.MapPost("/Generos", ([FromServices] DAL<Genero> DAL, [FromBody] GeneroRequest generoRequest) =>
        {
            var generoExistente = DAL.RecuperarPor(g => g.Nome!.ToUpper().Equals(generoRequest.Nome.ToUpper()));
            if (generoExistente is not null) return Results.Conflict("Esse gênero já existe!");

            var genero = new Genero()
            {
                Nome = generoRequest.Nome,
                Descricao = generoRequest.Descricao
            };

            DAL.Adicionar(genero);
            return Results.Ok();
        });

        app.MapDelete("/Generos/{id}", ([FromServices] DAL<Genero> DAL, int id) =>
        {
            var genero = DAL.RecuperarPor(g => g.Id == id);
            if (genero is null) return Results.NotFound();
            DAL.Deletar(genero);
            return Results.NoContent();
        });

        app.MapPut("/Generos", ([FromServices] DAL<Genero> DAL, [FromBody] GeneroRequestEdit generoRequestEdit) =>
        {
            var generoAtual = DAL.RecuperarPor(g => g.Id == generoRequestEdit.Id);
            if (generoAtual is null) return Results.NotFound();

            generoAtual.Nome = string.IsNullOrWhiteSpace(generoRequestEdit.Nome) ? generoAtual.Nome : generoRequestEdit.Nome;
            generoAtual.Descricao = string.IsNullOrWhiteSpace(generoRequestEdit.Descricao) ? generoAtual.Descricao : generoRequestEdit.Descricao;
            
            DAL.Atualizar(generoAtual);
            return Results.NoContent();
        });
    }

    private static ICollection<GeneroResponse> EntityListToResponseList(IEnumerable<Genero> generoList)
    {
        return generoList.Select(EntityToResponse).ToList();
    }

    private static GeneroResponse EntityToResponse(Genero genero)
    {
        return new GeneroResponse(genero.Id, genero.Nome!, genero.Descricao!);
    }
}
