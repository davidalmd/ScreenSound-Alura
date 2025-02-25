using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
var app = builder.Build();

app.MapGet("/", () => "API do projeto ScreenSound está rondando!");

app.MapGet("/Artistas", () =>
{
    var DAL = new DAL<Artista>(new ScreenSoundContext());
    return Results.Ok(DAL.Listar());
});

app.MapGet("/Artistas/{nome}", (string nome) =>
{
    var DAL = new DAL<Artista>(new ScreenSoundContext());
    var artista = DAL.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));

    if (artista is null) return Results.NotFound();

    var artistaLimpo = new Artista(artista.Nome, artista.Bio)
    {
        Id = artista.Id,
    };

    return Results.Ok(artistaLimpo);
});

app.MapPost("/Artistas", ([FromBody] Artista artista) =>
{
    var DAL = new DAL<Artista>(new ScreenSoundContext());
    DAL.Adicionar(artista);
    return Results.Ok();
});

app.Run();
