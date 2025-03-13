using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.API.Endpoints;

public static class MusicasExtensions
{
    public static void AddEndpointsMusicas(this WebApplication app)
    {
        app.MapGet("/Musicas", ([FromServices] DAL<Musica> DAL) =>
        {
            return Results.Ok(EntityListToResponseList(DAL.Listar()));
        });

        app.MapGet("/Musicas/{nome}", ([FromServices] DAL<Musica> DAL, string nome) =>
        {
            var musica = DAL.RecuperarPor(m => m.Nome.ToUpper().Equals(nome.ToUpper()));
            if (musica is null) return Results.NotFound();
            var musicaLimpa = new Musica(musica.Nome, musica.AnoLancamento)
            {
                Id = musica.Id,
                ArtistaId = musica.ArtistaId
            };
            return Results.Ok(EntityToResponse(musicaLimpa));
        });

        app.MapPost("/Musicas", ([FromServices] DAL<Musica> DAL, [FromBody] MusicaRequest musicaRequest) =>
        {
            var musica = new Musica(musicaRequest.Nome, musicaRequest.AnoLancamento)
            {
                ArtistaId = musicaRequest.ArtistaId
            };
            DAL.Adicionar(musica);
            return Results.Ok();
        });

        app.MapDelete("/Musicas/{id}", ([FromServices] DAL<Musica> DAL, int id) =>
        {
            var musica = DAL.RecuperarPor(m => m.Id == id);
            if (musica is null) return Results.NotFound();
            DAL.Deletar(musica);
            return Results.NoContent();
        });

        app.MapPut("/Musicas", ([FromServices] DAL<Musica> DAL, [FromBody] MusicaRequestEdit musicaRequestEdit) =>
        {
            var musicaAtual = DAL.RecuperarPor(m => m.Id == musicaRequestEdit.Id);
            if (musicaAtual is null) return Results.NotFound();

            musicaAtual.Nome = string.IsNullOrWhiteSpace(musicaRequestEdit.Nome) ? musicaAtual.Nome : musicaRequestEdit.Nome;
            musicaAtual.AnoLancamento = string.IsNullOrWhiteSpace(musicaRequestEdit.AnoLancamento.ToString()) ? musicaAtual.AnoLancamento : musicaRequestEdit.AnoLancamento;
            musicaAtual.ArtistaId = string.IsNullOrWhiteSpace(musicaRequestEdit.ArtistaId.ToString()) ? musicaAtual.ArtistaId : musicaRequestEdit.ArtistaId;

            DAL.Atualizar(musicaAtual);
            return Results.NoContent();
        });
    }

    private static ICollection<MusicaResponse> EntityListToResponseList(IEnumerable<Musica> musicaList)
    {
        return musicaList.Select(a => EntityToResponse(a)).ToList();
    }

    private static MusicaResponse EntityToResponse(Musica musica)
    {
        return new MusicaResponse(musica.Id, musica.Nome!, musica.AnoLancamento!.Value, musica.Artista!.Nome, musica.ArtistaId!.Value);
    }
}
