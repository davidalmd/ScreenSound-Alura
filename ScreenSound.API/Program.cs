using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ScreenSound.API.Endpoints;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using ScreenSound.Shared.Modelos.Modelos;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<ScreenSoundContext>( (options) =>
{
    options
        .UseSqlServer(builder.Configuration["ConnectionStrings:ScreenSoundDB"])
        .UseLazyLoadingProxies();
});
builder.Services.AddTransient<DAL<Artista>>();
builder.Services.AddTransient<DAL<Musica>>();
builder.Services.AddTransient<DAL<Genero>>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/", () => "API do projeto ScreenSound est� rondando!");

app.AddEndpointsArtistas();
app.AddEndpointsMusicas();
app.AddEndpointsGeneros();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
