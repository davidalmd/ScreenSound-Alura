using System.ComponentModel.DataAnnotations;

namespace ScreenSound.API.Requests;

public record ArtistaRequestEdit(
    [Required] int Id,
    string? Nome,
    string? FotoPerfil,
    string? Bio);
