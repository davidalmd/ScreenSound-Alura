﻿using System.ComponentModel.DataAnnotations;

namespace ScreenSound.API.Requests;

public record MusicaRequest(
    [Required] string Nome,
    [Required] int AnoLancamento,
    [Required] int ArtistaId);
