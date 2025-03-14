using System.ComponentModel.DataAnnotations;

namespace ScreenSound.API.Requests;

public record GeneroRequestEdit(
    [Required] int Id,
    string? Nome,
    string? Descricao);
