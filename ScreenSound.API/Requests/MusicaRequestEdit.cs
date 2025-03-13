using System.ComponentModel.DataAnnotations;

namespace ScreenSound.API.Requests;

public record MusicaRequestEdit(
    [Required] int Id,
    string? Nome,
    int? AnoLancamento,
    int? ArtistaId);
