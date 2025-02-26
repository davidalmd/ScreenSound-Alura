using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<ScreenSoundContext>();
builder.Services.AddTransient<DAL<Artista>>();

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

app.Run();
