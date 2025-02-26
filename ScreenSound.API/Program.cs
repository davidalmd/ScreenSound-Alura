using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<ScreenSoundContext>();
builder.Services.AddTransient<DAL<Artista>>();
builder.Services.AddTransient<DAL<Musica>>();

var app = builder.Build();

app.MapGet("/", () => "API do projeto ScreenSound está rondando!");

#region Endpoints de Artistas

app.MapGet("/Artistas", ([FromServices] DAL<Artista> DAL) =>
{
    return Results.Ok(DAL.Listar());
});

app.MapGet("/Artistas/{nome}", ([FromServices] DAL<Artista> DAL, string nome) =>
{
    var artista = DAL.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
    if (artista is null) return Results.NotFound();
    var artistaLimpo = new Artista(artista.Nome, artista.Bio)
    {
        Id = artista.Id,
    };
    return Results.Ok(artistaLimpo);
});

app.MapPost("/Artistas", ([FromServices] DAL<Artista> DAL, [FromBody] Artista artista) =>
{
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

app.MapPut("/Artistas", ([FromServices] DAL<Artista> DAL, [FromBody] Artista artista) =>
{
    var artistaAtual = DAL.RecuperarPor(a => a.Id == artista.Id);
    if (artistaAtual is null) return Results.NotFound();

    artistaAtual.Nome = artista.Nome;
    artistaAtual.Bio = artista.Bio;
    artistaAtual.FotoPerfil = artista.FotoPerfil;

    DAL.Atualizar(artistaAtual);
    return Results.NoContent();
});

#endregion

#region Endpoints de Músicas

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

#endregion

app.Run();
