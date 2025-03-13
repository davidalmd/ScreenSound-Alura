using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.API.Endpoints;

public static class ArtistasExtensions
{
    public static void AddEndpointsArtistas(this WebApplication app)
    {
        app.MapGet("/Artistas", ([FromServices] DAL<Artista> DAL) =>
        {
            return Results.Ok(EntityListToResponseList(DAL.Listar()));
        });

        app.MapGet("/Artistas/{nome}", ([FromServices] DAL<Artista> DAL, string nome) =>
        {
            var artista = DAL.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
            if (artista is null) return Results.NotFound();
            var artistaLimpo = new Artista(artista.Nome, artista.Bio)
            {
                Id = artista.Id,
            };
            return Results.Ok(EntityToResponse(artistaLimpo));
        });

        app.MapPost("/Artistas", ([FromServices] DAL<Artista> DAL, [FromBody] ArtistaRequest artistaRequest) =>
        {
            var artista = new Artista(artistaRequest.Nome, artistaRequest.Bio);
            DAL.Adicionar(artista);
            return Results.Ok();
        });

        app.MapDelete("/Artistas/{id}", ([FromServices] DAL<Artista> DAL, int id) =>
        {
            var artista = DAL.RecuperarPor(a => a.Id == id);
            if (artista is null) return Results.NotFound();

            DAL.Deletar(artista);
            return Results.NoContent();
        });

        app.MapPut("/Artistas", ([FromServices] DAL<Artista> DAL, [FromBody] ArtistaRequestEdit artistaRequestEdit) =>
        {
            var artistaAtual = DAL.RecuperarPor(a => a.Id == artistaRequestEdit.Id);
            if (artistaAtual is null) return Results.NotFound();

            artistaAtual.Nome = string.IsNullOrWhiteSpace(artistaRequestEdit.Nome) ? artistaAtual.Nome : artistaRequestEdit.Nome;
            artistaAtual.Bio = string.IsNullOrWhiteSpace(artistaRequestEdit.Bio) ? artistaAtual.Bio : artistaRequestEdit.Bio;
            artistaAtual.FotoPerfil = string.IsNullOrWhiteSpace(artistaRequestEdit.FotoPerfil) ? artistaAtual.FotoPerfil : artistaRequestEdit.FotoPerfil;

            DAL.Atualizar(artistaAtual);
            return Results.NoContent();
        });
    }

    private static ICollection<ArtistaResponse> EntityListToResponseList(IEnumerable<Artista> listaDeArtistas)
    {
        return listaDeArtistas.Select(a => EntityToResponse(a)).ToList();
    }

    private static ArtistaResponse EntityToResponse(Artista artista)
    {
        return new ArtistaResponse(artista.Id, artista.Nome, artista.Bio, artista.FotoPerfil);
    }
}
