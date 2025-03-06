using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.API.Endpoints;

public static class MusicasExtensions
{
    public static void AddEndpointsMusicas(this WebApplication app)
    {
        app.MapGet("/Musicas", ([FromServices] DAL<Musica> DAL) =>
        {
            return Results.Ok(DAL.Listar());
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
            return Results.Ok(musicaLimpa);
        });

        app.MapPost("/Musicas", ([FromServices] DAL<Musica> DAL, [FromBody] Musica musica) =>
        {
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

        app.MapPut("/Musicas", ([FromServices] DAL<Musica> DAL, [FromBody] Musica musica) =>
        {
            var musicaAtual = DAL.RecuperarPor(m => m.Id == musica.Id);
            if (musicaAtual is null) return Results.NotFound();
            musicaAtual.Nome = musica.Nome;
            musicaAtual.AnoLancamento = musica.AnoLancamento;
            musicaAtual.ArtistaId = musica.ArtistaId;
            DAL.Atualizar(musicaAtual);
            return Results.NoContent();
        });
    }
}
